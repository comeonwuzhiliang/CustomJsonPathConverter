using JsonPathConverter.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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
            destinationJsonColumns.Add(new DestinationJsonColumn { Code = "Logs", Name = "用户日志" });

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
                DestinationJsonColumnCode = "UserLogs",
                SourceJsonPath = "$.Logs"
            });
        }
    }
}
