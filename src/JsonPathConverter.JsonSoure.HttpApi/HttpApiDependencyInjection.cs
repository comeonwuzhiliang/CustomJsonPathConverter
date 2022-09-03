using JsonPathConverter.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Net;

namespace JsonPathConverter.JsonSoure.HttpApi
{
    public static class HttpApiDependencyInjection
    {
        private static IServiceCollection AddHttpApiClientAttachToken(this IServiceCollection serviceCollection)
        {
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

        public static IServiceCollection AddHttpApiJsonDataProviderAttachToken(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpApiClientAttachToken();
            serviceCollection.AddSingleton<IJsonDataProvider, HttpApiJsonDataProvider>();
            return serviceCollection;
        }
    }
}
