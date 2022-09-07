using JsonPathConverter.Abstractions;
using JsonPathConverter.JsonSource.HttpApi.Token;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Net;

namespace JsonPathConverter.JsonSoure.HttpApi
{
    public static class HttpApiDependencyInjection
    {
        private static IServiceCollection AddHttpApiClientWithToken(this IServiceCollection serviceCollection, Action<TokenClientOptions>? tokenClientOptions = null)
        {
            if (tokenClientOptions == null)
            {
                serviceCollection.Configure(new Action<TokenClientOptions>(s => { s.GrantType = ""; }));
            }
            else
            {
                serviceCollection.Configure(tokenClientOptions);
            }

            serviceCollection.AddTokenService("HttpApiJsonDataProvider_TokenClient");

            serviceCollection.AddHttpClient("HttpApiJsonDataProvider_RequestJsonDataProviderUri")
                .AddHttpMessageHandler(sp => {
                    var tokenService = sp.GetService<ITokenService>();
                    return ActivatorUtilities.CreateInstance<AccessTokenDelegatingHandler>(sp, tokenService!);
                })
                .AddTransientHttpErrorPolicy(builder =>
                        builder.WaitAndRetryAsync(new[]
                        {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(3)
                        }))
            .ConfigurePrimaryHttpMessageHandler(provider => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.All
            });

            return serviceCollection;
        }

        private static IServiceCollection AddHttpApiClient(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient("HttpApiJsonDataProvider_RequestJsonDataProviderUri")
                .ConfigurePrimaryHttpMessageHandler(provider => new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.All
                });

            return serviceCollection;
        }

        public static IServiceCollection AddHttpApiJsonDataProvider(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpApiClient();
            serviceCollection.AddSingleton<IJsonDataProvider, HttpApiJsonDataProvider>();
            return serviceCollection;
        }

        public static IServiceCollection AddHttpApiJsonDataProviderWithToken(this IServiceCollection serviceCollection, Action<TokenClientOptions>? tokenClientOptions = null)
        {
            serviceCollection.AddHttpApiClientWithToken(tokenClientOptions);
            serviceCollection.AddSingleton<IJsonDataProvider, HttpApiJsonDataProvider>();
            return serviceCollection;
        }
    }
}
