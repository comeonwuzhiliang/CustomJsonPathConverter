using JsonPathConverter.Abstractions;
using Newtonsoft.Json;
using Xunit;

namespace JsonPathConverter.ColumnMapper.NewObject.Test
{
    public class ColumnMapperNewObjectTest
    {
        [Fact]
        public void NewObject()
        {
            string json = "{\"totalCount\":2,\"items\":[{\"id\":1,\"name\":\"azir\",\"age\":27,\"areaId\":11,\"areaName\":\"江苏省盐城市\",\"hyperlinkUrl\":\"www.baidu.com\",\"hyperlinkName\":\"百度一下\",\"interalDepartment\":[{\"id\":111,\"name\":\"开发部\"},{\"id\":1110,\"name\":\"销售部\"}],\"start\":\"2022-10-01\",\"end\":\"2022-10-02\",\"time\":{\"start\":\"10:00\",\"end\":\"10:02\"}},{\"id\":2,\"name\":\"zack\",\"age\":28,\"areaId\":22,\"areaName\":\"江苏省苏州市\",\"hyperlinkUrl\":\"www.google.cn.hk\",\"hyperlinkName\":\"谷歌\",\"interalDepartment\":[{\"id\":222,\"name\":\"财务\"}],\"start\":\"2022-10-03\",\"end\":\"2022-10-05\",\"time\":{\"start\":\"10:06\",\"end\":\"10:08\"}}]}";

            JsonPathRoot jsonPathRoot = new JsonPathRoot("$.items");

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "Name", SourceJsonPath = "$.name" });

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "Age", SourceJsonPath = "$.age" });

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation
            {
                DestinationJsonColumnCode = "Area",
                SourceJsonPath = "$",
                DestinationPropertyType = DestinationPropertyTypeEnum.Array,
                ChildRelations = new List<JsonPathMapperRelation>
                {
                     new JsonPathMapperRelation{ DestinationJsonColumnCode = "Id", SourceJsonPath = "$.areaId" },
                     new JsonPathMapperRelation{ DestinationJsonColumnCode = "Name", SourceJsonPath = "$.areaName" }
                }
            });

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation
            {
                DestinationJsonColumnCode = "Hyperlink",
                SourceJsonPath = "$",
                DestinationPropertyType = DestinationPropertyTypeEnum.Array,
                ChildRelations = new List<JsonPathMapperRelation>
                {
                     new JsonPathMapperRelation{ DestinationJsonColumnCode = "Url", SourceJsonPath = "$.hyperlinkUrl" },
                     new JsonPathMapperRelation{ DestinationJsonColumnCode = "Name", SourceJsonPath = "$.hyperlinkName" }
                }
            });

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation
            {
                DestinationJsonColumnCode = "Department",
                SourceJsonPath = "$.interalDepartment",
                DestinationPropertyType = DestinationPropertyTypeEnum.Array,
                ChildRelations = new List<JsonPathMapperRelation>
                {
                     new JsonPathMapperRelation{ DestinationJsonColumnCode = "Id", SourceJsonPath = "$.id" },
                     new JsonPathMapperRelation{ DestinationJsonColumnCode = "Name", SourceJsonPath = "$.name" }
                }
            }); ;

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation
            {
                DestinationJsonColumnCode = "DateRange",
                SourceJsonPath = "$",
                DestinationPropertyType = DestinationPropertyTypeEnum.Object,
                ChildRelations = new List<JsonPathMapperRelation>
                {
                     new JsonPathMapperRelation{ DestinationJsonColumnCode = "Start", SourceJsonPath = "$.start" },
                     new JsonPathMapperRelation{ DestinationJsonColumnCode = "End", SourceJsonPath = "$.end" }
                }
            });

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation
            {
                DestinationJsonColumnCode = "TimeRange",
                SourceJsonPath = "$.time",
                DestinationPropertyType = DestinationPropertyTypeEnum.Object,
                ChildRelations = new List<JsonPathMapperRelation>
                {
                     new JsonPathMapperRelation{ DestinationJsonColumnCode = "Start", SourceJsonPath = "$.start" },
                     new JsonPathMapperRelation{ DestinationJsonColumnCode = "End", SourceJsonPath = "$.end" }
                }
            });

            var result = new ColumnMapperNewObject().MapToCollection(json, jsonPathRoot);

            Assert.Equal(2, result.Count());

            var systemTestJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.NotNull(systemTestJson);
        }
    }
}
