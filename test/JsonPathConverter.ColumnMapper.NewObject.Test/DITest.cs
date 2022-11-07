using JsonPathConverter.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace JsonPathConverter.ColumnMapper.NewObject.Test
{
    public class DITest
    {
        [Fact]
        public void JsonTemplate()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddColumnMapperNewObject();

            services.AddSingleton(new CurrentUser { UserId = 10086 });

            services.AddSingleton(new JsonPropertyFormatFunction
            {
                FormatKey = "guid",
                FormatFunction = new Func<object>(() => Guid.NewGuid())
            });

            services.AddSingleton(sp => new JsonPropertyFormatFunction
            {
                FormatKey = "CurrentUserId",
                FormatFunction = new Func<object>(() =>
                {
                    return sp.GetService<CurrentUser>()?.UserId ?? 0;
                })
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                string jsonTemplate = "{\"id\":\"Guid\",\"id2\":\"Guid\",\"userId\":\"CurrentUserId\",\"name\":\"$.name\"}";

                string jsonSource = "{\"name\":\"azir\"}";

                var jsonColumnMapper = scope.ServiceProvider.GetService<IJsonColumnMapper>();

                var obj = jsonColumnMapper!.MapToObjectByTemplate(jsonTemplate, jsonSource);

                Assert.IsType<Guid>(obj!.Data!["id"]);
                Assert.IsType<Guid>(obj!.Data!["id2"]);

                Assert.NotEqual(obj!.Data!["id"], obj!.Data!["id2"]);

                Assert.IsType<long>(obj!.Data!["userId"]);

                Assert.Equal("10086", obj!.Data!["userId"]!.ToString());
            }
        }

        public class CurrentUser
        {
            public long UserId { get; set; }
        }
    }
}