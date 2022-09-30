using JsonPathConverter.Abstractions;
using JsonPathConverter.JsonSource.HttpApi.Abstractions;
using JsonPathConverter.JsonSoure.HttpApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Xunit;

namespace JsonPathConverter.JsonSource.HttpApi.Test
{
    /// <summary>
    /// 测试依赖ids服务器，这边我用的是基于ABP官方的DEMO扩展的一些其他功能的代码
    /// github地址：https://github.com/comeonwuzhiliang/ABP.DEMO
    /// 只需要运行Host项目文件就行了
    /// </summary>
    public class PasswordTest
    {
        public static IConfiguration? Configuration;

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(s =>
        {
            s.AddJsonFile("appsettings.Oauth.json");
            Configuration = s.Build();
        });

        [Fact]
        [Trait("Category", "NoRunTest")] //CI里面记得加 dotnet test --filter Category!=NoRunTest,或者可以[Fact=skip("")]
        public async void OauthTokenTestAsync()
        {
            var hostBuilder = CreateHostBuilder();

            hostBuilder.ConfigureServices(serviceCollection =>
            {
                serviceCollection.AddHttpApiJsonDataProviderWithToken(Configuration!.GetSection("password:TokenClient").Bind);

                serviceCollection.AddLogging();
            });

            var build = hostBuilder.Build();

            using (var scope = build.Services.CreateScope())
            {
                string requestUrl = Configuration!["password:ClientUrl"];

                var httpFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
                var client = httpFactory.CreateClient();
                var response = await client.GetAsync(requestUrl);

                Assert.True(response.StatusCode != System.Net.HttpStatusCode.OK);

                var jsonDataProvider = scope.ServiceProvider.GetRequiredService<IJsonDataProvider>();

                var jsonData = await jsonDataProvider.GetJsonDataAsync(new JsonHttpApiRequestSource(new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestUrl)
                }));

                var jsonDataDic = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonData);

                Assert.True(int.Parse(jsonDataDic!["totalCount"].ToString()!) > 0);

            }
        }
    }
}
