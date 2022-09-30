using JsonPathConverter.Abstractions;
using JsonPathConverter.ColumnMapper.ReplaceKey;
using JsonPathConverter.FakeObject;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonPathConverter.HttpApi.Test
{
    public class ColumnMapperReplaceKeyTest
    {
        [Fact]
        public void JsonPathMatchColumnSingleObject()
        {
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

            jsonPathMapperRelations.Add(new JsonPathMapperRelation()
            {
                DestinationJsonColumnCode = "OtherRemark",
                SourceJsonPath = "$.OtherInformation.Remark"
            });

            // fake source json object
            UserAction userAction = new UserAction
            {
                UserId = 1,
                UserName = "pluma",
                OtherInformation = new UserOtherInformation { Remark = "pluma login system" },
                UserLogs = new List<UserLog>{
                    new UserLog{ ActionName = "Login System", Message ="pluma login in to the system on the web", Date = DateTime.Parse("2022-08-29 10:00:00") },
                    new UserLog{ ActionName = "Login out System", Message ="pluma login out of the web",  Date = DateTime.Parse("2022-08-29 10:01:00")}
                }
            };

            // fake json source
            string userActionsJsonStr = JsonConvert.SerializeObject(userAction);

            JsonPathRoot jsonPathRoot = new JsonPathRoot("$");
            jsonPathMapperRelations.ForEach(s => jsonPathRoot.AddJsonPathMapper(s));

            var jsonMapper = new ColumnMapperReplaceKey().MapToCollection(userActionsJsonStr, jsonPathRoot);

            Assert.NotEmpty(jsonMapper);
        }

        [Fact]
        public void JsonPathMatchColumnMutipleArray()
        {
            List<JsonPathMapperRelation> jsonPathMapperRelations = new List<JsonPathMapperRelation>();
            jsonPathMapperRelations.Add(new JsonPathMapperRelation()
            {
                DestinationJsonColumnCode = "Id",
                SourceJsonPath = "$[*].UserId"
            });

            jsonPathMapperRelations.Add(new JsonPathMapperRelation()
            {
                DestinationJsonColumnCode = "Name",
                SourceJsonPath = "$[0].UserName"
            });

            jsonPathMapperRelations.Add(new JsonPathMapperRelation()
            {
                DestinationJsonColumnCode = "LogsMessage",
                SourceJsonPath = "$[*].UserLogs[*].Message"
            });

            jsonPathMapperRelations.Add(new JsonPathMapperRelation()
            {
                DestinationJsonColumnCode = "TestDate",
                SourceJsonPath = "$.UserLogs[0].Date"
            });

            jsonPathMapperRelations.Add(new JsonPathMapperRelation()
            {
                DestinationJsonColumnCode = "MotionName",
                SourceJsonPath = "$[*].UserLogs[0].ActionName"
            });

            jsonPathMapperRelations.Add(new JsonPathMapperRelation()
            {
                DestinationJsonColumnCode = "MotionNameCopy",
                SourceJsonPath = "$.UserLogs.ActionName"
            });

            // fake source json object

            UserAction[] userAction = new UserAction[]
            {
                new UserAction
                {
                    UserId = 1,
                    UserName = "pluma",
                    OtherInformation = new UserOtherInformation { Remark = "pluma login system" },
                    UserLogs = new List<UserLog>
                    {
                        new UserLog{ ActionName = "Login System", Message ="pluma login in to the system on the web", Date = DateTime.Parse("2022-08-29 10:00:00") },
                        new UserLog{ ActionName = "Login out System", Message ="pluma login out of the web",  Date = DateTime.Parse("2022-08-29 10:01:00")}
                    }
                },
                new UserAction
                {
                    UserId = 2,
                    UserName = "fizz",
                    OtherInformation = new UserOtherInformation { Remark = "fizz login system" },
                    UserLogs = new List<UserLog>
                    {
                        new UserLog{ ActionName = "Login System", Message ="fizz login in to the system on the web", Date = DateTime.Parse("2022-08-29 10:00:00") },
                        new UserLog{ ActionName = "Login out System", Message ="fizz login out of the web",  Date = DateTime.Parse("2022-08-29 10:01:00")}
                    }
                },
            };

            // fake json source
            string userActionsJsonStr = JsonConvert.SerializeObject(userAction);

            JsonPathRoot jsonPathRoot = new JsonPathRoot("$");
            jsonPathMapperRelations.ForEach(s => jsonPathRoot.AddJsonPathMapper(s));

            var mapResult = new ColumnMapperReplaceKey().MapToCollection(userActionsJsonStr, jsonPathRoot);

            var destinationJsonStr = JsonConvert.SerializeObject(mapResult);

            JToken token = JToken.Parse(destinationJsonStr);

            var tokens_1 = token.SelectTokens("$[*].Id").ToList();
            Assert.True(2 == tokens_1.Count);

            var tokens_2 = token.SelectTokens("$[0].Name").ToList();
            Assert.True(1 == tokens_2.Count);

            var tokens_3 = token.SelectTokens("$[*].UserLogs[*].LogsMessage").ToList();
            Assert.True(4 == tokens_3.Count);

            var tokens_4 = token.SelectTokens("$[*].UserLogs[0].MotionName").ToList();
            Assert.True(2 == tokens_4.Count);
        }
    }
}
