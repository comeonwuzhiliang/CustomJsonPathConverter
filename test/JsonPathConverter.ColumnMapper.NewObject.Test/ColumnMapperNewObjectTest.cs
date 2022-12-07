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

            Assert.Equal(2, result.Data?.Count());

            var systemTestJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.NotNull(systemTestJson);
        }

        [Fact]
        public void Collection()
        {
            string json = "{\"data\":{\"message\":\"操作成功。\",\"totalItems\":0,\"startIndex\":0,\"itemsPerPage\":0,\"items\":[{\"c_id\":\"9b596bf7-3b90-49df-96f6-596ec9d7791b\",\"piao_zheng_lei_xing\":[{\"label\":\"X2许可证\",\"value\":\"ba7b52cb-2032-4dc3-8191-6ab5a4408e32\",\"defineType\":11}],\"zuo_ye_piao_ming_cheng\":\"wuzhiliangTest\",\"zuo_ye_piao_zhuang_tai\":\"待开票\",\"piao_hao\":[{\"label\":\"99-NAR-2022-1206-008\",\"defineType\":20}],\"zuo_ye_fu_zhai_ren\":[],\"chu_li_ren\":[{\"c_id\":\"20e4b904-300c-4b04-99c3-93a9ac68e048\",\"c_id2\":\"20e4b904-300c-4b04-99c3-93a9ac68e048\",\"name\":\"azir\",\"mobile\":\"\",\"deptid\":\"1\",\"deptname\":null,\"gender\":1,\"functionposttitle\":\"1a007f28-b859-4d86-80ee-9ae4738e5c47\",\"worktype\":null,\"walkietalkie\":null,\"officename\":\"\",\"joysuchmac\":null,\"xh\":0}]}]},\"isUseCache\":0,\"IsSuccess\":true}";
            JsonPathRoot jsonPathRoot = new JsonPathRoot("$.data.items");
            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation
            {
                DestinationJsonColumnCode = "chu_li_ren",
                DestinationPropertyType = DestinationPropertyTypeEnum.Array,
                SourceJsonPath = "$.chu_li_ren",
                ChildRelations = new List<JsonPathMapperRelation> { 
                    new JsonPathMapperRelation { DestinationJsonColumnCode = "id", SourceJsonPath = "$.c_id" },
                    new JsonPathMapperRelation { DestinationJsonColumnCode = "name", SourceJsonPath = "$.name" }
                }
            });

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation
            {
                DestinationJsonColumnCode = "zuo_ye_piao_ming_cheng",
                DestinationPropertyType = DestinationPropertyTypeEnum.Property,
                SourceJsonPath = "$.zuo_ye_piao_ming_cheng"
            });

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation
            {
                DestinationJsonColumnCode = "piao_zheng_lei_xing_2",
                DestinationPropertyType = DestinationPropertyTypeEnum.Array,
                SourceJsonPath = "$.piao_zheng_lei_xing.value"
            });

            var result = new ColumnMapperNewObject().MapToCollection(json, jsonPathRoot);

            Assert.NotNull(result);

            var values = result.Data!.First()["piao_zheng_lei_xing_2"] as List<object>;

            Assert.True(values!.Any());

            Assert.Equal("ba7b52cb-2032-4dc3-8191-6ab5a4408e32", values![0]);
        }
    }
}
