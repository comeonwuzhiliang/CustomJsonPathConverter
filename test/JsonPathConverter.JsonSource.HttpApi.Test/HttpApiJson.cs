using JsonPathConverter.Abstractions;
using JsonPathConverter.ColumnMapper.NewObject;
using JsonPathConverter.JsonSource.HttpApi.Abstractions;
using JsonPathConverter.JsonSource.HttpApi.Oauth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace JsonPathConverter.JsonSource.HttpApi.Test
{
    public class HttpApiJson
    {
        [Fact]
        public void DI()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddHttpApiJsonDataProviderWithToken(s => s.GrantType = "client_credentials");

            serviceCollection.AddColumnMapperNewObject();

            serviceCollection.AddLogging();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var tokenService = scope.ServiceProvider.GetService<ITokenService>()!;
                Assert.True(tokenService.GetType() == typeof(TokenService));

                var tokenOptions = scope.ServiceProvider.GetService<IOptions<TokenClientOptions>>();

                Assert.True(scope.ServiceProvider.GetService<IJsonColumnMapper>()!.GetType() == typeof(ColumnMapperNewObject));
                Assert.True(scope.ServiceProvider.GetService<IJsonDataProvider>()!.GetType() == typeof(HttpApiJsonDataProvider));
            }
        }

        [Fact]
        public async void SendHttpRequest()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddHttpApiJsonDataProvider();

            serviceCollection.AddColumnMapperNewObject();

            serviceCollection.AddLogging();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                IJsonDataProvider jsonDataProvider = scope.ServiceProvider.GetService<IJsonDataProvider>()!;
                JsonPathRoot jsonPathRoot = new JsonPathRoot("$.pages");

                jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "PageName", SourceJsonPath = "$.name" });
                jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "PageUrl", SourceJsonPath = "$.url" });
                jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "PageNamespace", SourceJsonPath = "$.namespace" });

                IJsonRequestSource requestSource = new JsonHttpApiRequestSource(new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri("https://s.alicdn.com/@xconfig/flasher_classic/manifest") });

                var apiJsonStr = await jsonDataProvider.GetJsonDataAsync(requestSource, default);

                IJsonColumnMapper jsonColumnMapper = scope.ServiceProvider.GetService<IJsonColumnMapper>()!;

                var resultJson = jsonColumnMapper.MapToCollection(apiJsonStr, jsonPathRoot);

                Assert.True(resultJson?.Data?.Any());
            }
        }
    }
}
