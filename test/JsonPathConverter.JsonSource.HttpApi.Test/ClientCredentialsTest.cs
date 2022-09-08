using JsonPathConverter.Abstractions;
using JsonPathConverter.JsonSource.HttpApi.Abstractions;
using JsonPathConverter.JsonSource.HttpApi.Token;
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
    public class ClientCredentialsTest
    {
        public static IConfiguration Configuration;

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
                serviceCollection.AddHttpApiJsonDataProviderWithToken(Configuration!.GetSection("client_credentials:TokenClient").Bind);

                serviceCollection.AddLogging();
            });

            var build = hostBuilder.Build();

            using (var scope = build.Services.CreateScope())
            {
                string requestUrl = Configuration["client_credentials:ClientUrl"];

                ITokenService tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

                string token = await tokenService.GetToken();

                Assert.NotNull(token);

                Assert.NotEmpty(token);
            }
        }
    }
}
