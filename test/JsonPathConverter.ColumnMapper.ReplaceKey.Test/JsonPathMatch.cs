using JsonPathConverter.Abstractions;
using JsonPathConverter.ColumnMapper.ReplaceKey;
using JsonPathConverter.Test.FakeObject;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Xunit;

namespace JsonPathConverter.HttpApi.Test
{
    public class JsonPathMatch
    {
        [Fact]
        public void JsonPathMatchColumnSingleObject()
        {
            List<DestinationJsonColumn> destinationJsonColumns = new List<DestinationJsonColumn>();

            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "Id", Name = "用户Id" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "Name", Name = "用户编号" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "LogsMessage", Name = "用户日志内容" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "OtherRemark", Name = "其他备注" });

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
            string userActionsJsonStr = JsonSerializer.Serialize(userAction);

            JsonPathRoot jsonPathRoot = new JsonPathRoot("$", destinationJsonColumns);
            jsonPathMapperRelations.ForEach(s => jsonPathRoot.AddJsonPathMapper(s));

            var jsonMapper = new ColumnMapperReplaceKey().Map<JToken>(userActionsJsonStr, jsonPathRoot);

            var destinationJsonStr = JsonSerializer.Serialize(jsonMapper.MapData);

            Assert.True(1 == 1);
        }

        [Fact]
        public void JsonPathMatchColumnMutipleArray()
        {
            List<DestinationJsonColumn> destinationJsonColumns = new List<DestinationJsonColumn>();

            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "Id", Name = "用户Id" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "Name", Name = "用户编号" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "LogsMessage", Name = "用户日志内容" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "OtherRemark", Name = "其他备注" });

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
                DestinationJsonColumnCode = "MotionName",
                SourceJsonPath = "$[*].UserLogs[0].ActionName"
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
            string userActionsJsonStr = JsonSerializer.Serialize(userAction);

            JsonPathRoot jsonPathRoot = new JsonPathRoot("$", destinationJsonColumns);
            jsonPathMapperRelations.ForEach(s => jsonPathRoot.AddJsonPathMapper(s));

            var mapResult = new ColumnMapperReplaceKey().Map<JToken>(userActionsJsonStr, jsonPathRoot);

            var destinationJsonStr = mapResult.MapJsonStr;

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
