using JsonPathConverter.HttpApi.Test.Fake;
using JsonPathConverter.Interface;
using Xunit;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Text.Json.JsonElement;
using JsonPathConverter.DefaultColumnMapper;

namespace JsonPathConverter.HttpApi.Test
{
    public class JsonPathMatch
    {
        [Fact]
        public void JsonPathMatchColumn()
        {
            List<DestinationJsonColumn> destinationJsonColumns = new List<DestinationJsonColumn>();

            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "Id", Name = "用户Id" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "Name", Name = "用户编号" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "LogsMessage", Name = "用户日志内容" });

            List<JsonPathMapperRelation> jsonPathMapperRelations = new List<JsonPathMapperRelation>();
            jsonPathMapperRelations.Add(new JsonPathMapperRelation()
            {
                DestinationJsonColumnCode = "Id",
                SourceJsonPath = "$.UserId"
            });

            jsonPathMapperRelations.Add(new JsonPathMapperRelation()
            {
                DestinationJsonColumnCode = "Name",
                SourceJsonPath = "$.UserName"
            });

            jsonPathMapperRelations.Add(new JsonPathMapperRelation()
            {
                DestinationJsonColumnCode = "LogsMessage",
                SourceJsonPath = "$.UserLogs.Message"
            });

            // fake source json object
            List<UserAction> userActions = new List<UserAction>()
            {
                new UserAction{ UserId =1, UserName ="pluma", UserLogs = new List<UserLog>{
                    new UserLog{ ActionName = "Login System", Message ="pluma login in to the system on the web", Date = DateTime.Parse("2022-08-29 10:00:00") },
                    new UserLog{ ActionName = "Login out System", Message ="pluma login out of the web",  Date = DateTime.Parse("2022-08-29 10:01:00")}
                } },
                new UserAction { UserId = 2, UserName ="redz",UserLogs = new List<UserLog>{
                    new UserLog{ ActionName = "Login System", Message ="redz login in to the system on the web", Date = DateTime.Parse("2022-08-29 10:00:00") },
                    new UserLog{ ActionName = "Login out System", Message ="redz login out of the web",  Date = DateTime.Parse("2022-08-29 10:01:00")}
                } },
                new UserAction{ UserId = 3,UserName ="fizz",UserLogs = new List<UserLog>{
                    new UserLog{ ActionName = "Login System", Message ="fizz login in to the system on the web", Date = DateTime.Parse("2022-08-29 10:00:00") },
                    new UserLog{ ActionName = "Login out System", Message ="fizz login out of the web",  Date = DateTime.Parse("2022-08-29 10:01:00")}
                } },
            };

            // fake json source
            string userActionsJsonStr = JsonSerializer.Serialize(userActions);

            var jsonSourceElements = JsonColumnMapper.JsonSourceElements(userActionsJsonStr, destinationJsonColumns, jsonPathMapperRelations);

            var destinationJsonResult = GenerateDestinationJsonStr(jsonSourceElements);

            Assert.True(1 == 1);
        }

        private string GenerateDestinationJsonStr(Dictionary<string, JsonElementDetail> jsonSourceElements)
        {
            // 构建对象
            Dictionary<string, object?> objectProperties = new Dictionary<string, object?>();

            var propertyNames = jsonSourceElements.Select(s => s.Key);

            foreach (var propertyName in propertyNames)
            {
                objectProperties[propertyName] = default;
            }

            foreach (var item in objectProperties)
            {

            }

            // 查找元素之间的对象关系
            foreach (var jsonSourceElement in jsonSourceElements)
            {
                foreach (var jsonElementRelation in jsonSourceElement.Value.JsonElementRelations)
                {
                    JsonElement jsonElement = jsonElementRelation.Self;

                    foreach (var ancestor in jsonElementRelation.Ancestors)
                    {

                    }
                }
            }

            return string.Empty;
        }
    }
}
