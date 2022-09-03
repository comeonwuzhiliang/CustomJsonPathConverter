using JsonPathConverter.Abstractions;
using JsonPathConverter.ColumnMapper.NewObject;
using JsonPathConverter.Test.FakeObject;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Xunit;

namespace JsonPathConverter.HttpApi.Test
{
    public class JsonPathMatch
    {
        [Fact]
        public void JsonPathMatchColumnMutipleArray()
        {
            List<DestinationJsonColumn> destinationJsonColumns = new List<DestinationJsonColumn>();

            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "Id", Name = "用户Id" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "Name", Name = "用户编号" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "LogsMessage", Name = "用户日志内容" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "OtherRemark", Name = "备注" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "OtherInfo", Name = "其他信息" });

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

            jsonPathMapperRelations.Add(new JsonPathMapperRelation()
            {
                DestinationJsonColumnCode = "OtherInfo",
                SourceJsonPath = "$.OtherInformation"
            });

            // fake source json object
            List<UserAction> userActions = new List<UserAction>()
            {
                new UserAction{
                    UserId =1,
                    UserName ="pluma",
                    OtherInformation =new UserOtherInformation{Remark = "pluma login system" },
                    UserLogs = new List<UserLog>{
                    new UserLog{ ActionName = "Login System", Message ="pluma login in to the system on the web", Date = DateTime.Parse("2022-08-29 10:00:00") },
                    new UserLog{ ActionName = "Login out System", Message ="pluma login out of the web",  Date = DateTime.Parse("2022-08-29 10:01:00")}
                } },

                new UserAction { UserId = 2, UserName ="redz",UserLogs = new List<UserLog>{
                }},
                new UserAction
                {
                    UserId = 3,
                    UserName = "fizz",
                    OtherInformation =new UserOtherInformation{Remark = "fizz login system" },
                    UserLogs = new List<UserLog>{
                    new UserLog{ ActionName = "Login System", Message ="fizz login in to the system on the web", Date = DateTime.Parse("2022-08-29 10:00:00") },
                    new UserLog{ ActionName = "Login out System", Message ="fizz login out of the web",  Date = DateTime.Parse("2022-08-29 10:01:00")}
                }
                },
            };

            // fake json source
            string userActionsJsonStr = JsonSerializer.Serialize(userActions);

            JsonPathRoot jsonPathRoot = new JsonPathRoot("$", destinationJsonColumns);
            jsonPathMapperRelations.ForEach(s => jsonPathRoot.AddJsonPathMapper(s));

            var jsonMapper = new ColumnMapperNewObject().MapToCollection(userActionsJsonStr, jsonPathRoot);

            var destinationJsonStr = JsonSerializer.Serialize(jsonMapper.MapData);

            List<IDictionary<string, object?>> destination = JsonSerializer.Deserialize<List<IDictionary<string, object?>>>(destinationJsonStr)!;

            Assert.Equal(userActions.Count, destination.Count());

            foreach (var item in destination)
            {
                var userAction = userActions.First(s => s.UserId.ToString() == item["Id"]!.ToString());

                Assert.Equal(userAction.UserName, item["Name"]!.ToString());

                var userActionLogsMessages = userAction.UserLogs.Select(s => s.Message);
                if (userActionLogsMessages.Any())
                {
                    var logsMessages = JsonSerializer.Deserialize<string[]>(item["LogsMessage"]!.ToString()!);
                    foreach (var userActionLogsMessage in userActionLogsMessages)
                    {
                        Assert.Contains(userActionLogsMessage, logsMessages);
                    }
                }

                if(item.ContainsKey("OtherRemark"))
                {
                    Assert.Equal(userAction.OtherInformation.Remark, item["OtherRemark"]!.ToString());
                }

                if (item.ContainsKey("OtherInfo"))
                {
                    var otherInfo = JsonSerializer.Deserialize<IDictionary<string, object?>>(item["OtherInfo"]!.ToString()!);

                    Assert.Equal(userAction.OtherInformation.Remark, otherInfo!["Remark"]!.ToString());
                }
            }
        }

        [Fact]
        public void JsonPathMatchColumnSingleObject()
        {
            List<DestinationJsonColumn> destinationJsonColumns = new List<DestinationJsonColumn>();

            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "Id", Name = "用户Id" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "Name", Name = "用户编号" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "LogsMessage", Name = "用户日志内容" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "OtherRemark", Name = "备注" });
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "OtherInfo", Name = "其他信息" });

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

            jsonPathMapperRelations.Add(new JsonPathMapperRelation()
            {
                DestinationJsonColumnCode = "OtherInfo",
                SourceJsonPath = "$.OtherInformation"
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

            var jsonMapper = new ColumnMapperNewObject().MapToCollection(userActionsJsonStr, jsonPathRoot);

            var destinationJsonStr = JsonSerializer.Serialize(jsonMapper.MapData);

            List<IDictionary<string, object?>> destination = JsonSerializer.Deserialize<List<IDictionary<string, object?>>>(destinationJsonStr)!;

            Assert.Single(destination);

            Assert.Equal(userAction.UserName, destination[0]["Name"]!.ToString());

            var userActionLogsMessages = userAction.UserLogs.Select(s => s.Message);
            if (userActionLogsMessages.Any())
            {
                var logsMessages = JsonSerializer.Deserialize<string[]>(destination[0]["LogsMessage"]!.ToString()!);
                foreach (var userActionLogsMessage in userActionLogsMessages)
                {
                    Assert.Contains(userActionLogsMessage, logsMessages);
                }
            }

            Assert.Equal(userAction.OtherInformation.Remark, destination[0]["OtherRemark"]!.ToString());

            var otherInfo = JsonSerializer.Deserialize<IDictionary<string, object?>>(destination[0]["OtherInfo"]!.ToString()!);

            Assert.Equal(userAction.OtherInformation.Remark, otherInfo!["Remark"]!.ToString());
        }
    }
}
