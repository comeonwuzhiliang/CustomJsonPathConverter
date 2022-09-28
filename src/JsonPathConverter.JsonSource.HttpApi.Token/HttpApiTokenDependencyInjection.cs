using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace JsonPathConverter.JsonSource.HttpApi.Oauth
{
    public static class HttpApiTokenDependencyInjection
    {
        public static IServiceCollection AddTokenService(this IServiceCollection services, string tokenClientName)
        {
            services.AddHttpClient(tokenClientName);

            services.TryAddSingleton<ITokenService>(sp =>
            {
                var options = sp.GetService<IOptions<TokenClientOptions>>();
                return sp.GetTokenService(options?.Value ?? new TokenClientOptions { }, tokenClientName);
            });
            return services;
        }

        private static TokenService GetTokenService(this IServiceProvider sp, TokenClientOptions clientOptions, string tokenClientName)
        {
            var tokenClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient(tokenClientName);

            Func<CancellationToken, Task<string>> tokenInvoker = clientOptions.GrantType switch
            {
                OidcConstants.GrantTypes.ClientCredentials => cancellationToken => ActivatorUtilities.CreateInstance<ClientCredentialsTokenClient>(sp, tokenClient)
                    .GetAccessTokenAsync(clientOptions.TokenRequest?.Get<ClientCredentialsTokenRequest>(), cancellationToken),

                OidcConstants.GrantTypes.AuthorizationCode => cancellationToken => ActivatorUtilities.CreateInstance<AuthorizationCodeTokenClient>(sp, tokenClient)
                    .GetAccessTokenAsync(clientOptions.TokenRequest?.Get<AuthorizationCodeTokenRequest>(), cancellationToken),

                OidcConstants.GrantTypes.Password => cancellationToken => ActivatorUtilities.CreateInstance<PasswordTokenClient>(sp, tokenClient)
                    .GetAccessTokenAsync(clientOptions.TokenRequest?.Get<PasswordTokenRequest>(), cancellationToken),

                OidcConstants.GrantTypes.DeviceCode => cancellationToken => ActivatorUtilities.CreateInstance<DeviceTokenClient>(sp, tokenClient)
                    .GetAccessTokenAsync(clientOptions.TokenRequest?.Get<DeviceTokenRequest>(), cancellationToken),

                OidcConstants.GrantTypes.RefreshToken => cancellationToken => ActivatorUtilities.CreateInstance<RefreshTokenClient>(sp, tokenClient)
                    .GetAccessTokenAsync(clientOptions.TokenRequest?.Get<RefreshTokenRequest>(), cancellationToken),

                _ => cancellationToken => ActivatorUtilities.CreateInstance<AttachTokenClient>(sp, sp.GetRequiredService<IHttpContextAccessor>())
                    .GetAccessTokenAsync(new AttachTokenRequest(), cancellationToken),
            };

            return new TokenService(tokenInvoker);
        }

    }
}
