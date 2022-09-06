using JsonPathConverter.Abstractions;
using JsonPathConverter.JsonSource.HttpApi.Token;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Net;

namespace JsonPathConverter.JsonSoure.HttpApi
{
    public static class HttpApiDependencyInjection
    {
        private static IServiceCollection AddHttpApiClientWithToken(this IServiceCollection serviceCollection, Action<TokenClientOptions> tokenClientOptions)
        {
            serviceCollection.Configure(tokenClientOptions);

            serviceCollection.AddHttpClient("HttpApiJsonDataProvider_RequestJsonDataProviderUri")
                .AddHttpMessageHandler<AccessTokenDelegatingHandler>()
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

            serviceCollection.AddTokenService("HttpApiJsonDataProvider_TokenClient");

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

        public static IServiceCollection AddHttpApiJsonDataProviderWithToken(this IServiceCollection serviceCollection, Action<TokenClientOptions> tokenClientOptions)
        {
            serviceCollection.AddHttpApiClientWithToken(tokenClientOptions);
            serviceCollection.AddSingleton<IJsonDataProvider, HttpApiJsonDataProvider>();
            return serviceCollection;
        }
    }
}
