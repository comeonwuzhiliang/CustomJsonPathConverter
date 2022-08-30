using JsonPathConverter.DefaultColumnMapper;
using JsonPathConverter.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Net;

namespace JsonPathConverter.HttpApi.DependencyInjection;
public static class JsonPathConverterHttpApiDependencyInjection
{
    public static IServiceCollection AddHttpApiClientAttachToken(this IServiceCollection serviceCollection)
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

    public static IServiceCollection AddHttpApiClient(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient("HttpApiJsonDataProvider_RequestJsonDataProviderUri")
            .ConfigurePrimaryHttpMessageHandler(provider => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.All
            });

        return serviceCollection;
    }

    public static IServiceCollection AddHttpApiJsonPathConverterAttachToken(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpApiJsonPathConverter();
        serviceCollection.AddHttpApiClientAttachToken();
        return serviceCollection;
    }

    public static IServiceCollection AddHttpApiJsonPathConverter(this IServiceCollection serviceCollection)
    {
        // add json data provider
        serviceCollection.AddScoped<IJsonDataProvider, HttpApiJsonDataProvider>();

        // add json column provider
        serviceCollection.AddSingleton<IJsonColumnMapper, SystemTextJsonColumnMapper>();

        serviceCollection.Configure<JsonPathColumnTypeMapperOption>(option =>
        {
            //option.JsonColumnTypeMappers.Add();
        });

        return serviceCollection;
    }
}
