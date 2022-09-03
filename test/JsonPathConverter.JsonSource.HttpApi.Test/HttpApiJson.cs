using JsonPathConverter.Abstractions;
using JsonPathConverter.ColumnMapper.NewObject;
using JsonPathConverter.ColumnMapper.ReplaceKey;
using JsonPathConverter.JsonSoure.HttpApi;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace JsonPathConverter.JsonSource.HttpApi.Test
{
    public class HttpApiJson
    {
        [Fact]
        public void DI()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddHttpApiJsonDataProvider();

            serviceCollection.AddColumnMapperReplaceKey();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                Assert.True(scope.ServiceProvider.GetService<IJsonColumnMapper>()!.GetType() == typeof(ColumnMapperReplaceKey));
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
                JsonPathRoot jsonPathRoot = new JsonPathHttpApiRoot("$.pages",
                     new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri("https://s.alicdn.com/@xconfig/flasher_classic/manifest") }
                    , new List<DestinationJsonColumn>()
                    {
                        new DestinationJsonColumn{ Code ="PageName",Name ="页面名称" },
                        new DestinationJsonColumn{ Code ="PageUrl",Name ="页面地址" },
                        new DestinationJsonColumn{ Code ="PageNamespace",Name ="页面空间" },
                        new DestinationJsonColumn{ Code ="id",Name ="Id" },
                    });

                jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "PageName", SourceJsonPath = "$.name", RootPath = jsonPathRoot.RootPath });
                jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "PageUrl", SourceJsonPath = "$.url", RootPath = jsonPathRoot.RootPath });
                jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "PageNamespace", SourceJsonPath = "$.namespace", RootPath = jsonPathRoot.RootPath });

                var result = await jsonDataProvider.GetJsonData(jsonPathRoot);

                var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

                Assert.True(!string.IsNullOrEmpty(resultJson));
            }
        }
    }
}
