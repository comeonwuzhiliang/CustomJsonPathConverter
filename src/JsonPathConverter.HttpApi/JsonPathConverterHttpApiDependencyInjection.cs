using JsonPathConverter.Interface;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace JsonPathConverter.HttpApi.DependencyInjection;
public static class JsonPathConverterHttpApiDependencyInjection
{
    public static IServiceCollection AddHttpApiJsonPathConverter(this IServiceCollection serviceCollection)
    {
        // add HttpClient
        serviceCollection.AddHttpClient("HttpApiJsonDataProvider_RequestJsonDataProviderUri")
            .AddHttpMessageHandler<AccessTokenDelegatingHandler>()
            .AddTransientHttpErrorPolicy(builder =>
                    builder.WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(3)
                    }));

        // add json data provider
        serviceCollection.AddScoped<IJsonDataProvider, HttpApiJsonDataProvider>();

        serviceCollection.Configure<JsonPathColumnTypeMapperOption>(option =>
        {
            //option.JsonColumnTypeMappers.Add();
        });

        return serviceCollection;
    }
}
