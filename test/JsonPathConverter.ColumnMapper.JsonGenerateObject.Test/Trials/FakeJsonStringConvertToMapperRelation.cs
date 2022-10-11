using JsonPathConverter.Abstractions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonPathConverter.ColumnMapper.JsonGenerateObject.Test.Trials;

public class FakeJsonStringConvertToMapperRelation
{
    [Fact]
    public void Mapper()
    {
        string jsonTemplate =
            "{\"name\":\"$.name\",\"createDataRange\":{\"start\":\"$.startCreateData\",\"end\":\"$.endCreateData\"},\"hyperLinkContext\":\"$.hyperLink.context\",\"hyperLinkUrl\":\"$.hyperLink.url\",\"departments\":[{\"id\":\"$.department.id\",\"name\":\"$.department.name\",\"leaders\":[\"$.leaders.name\"]}],\"role\":{\"id\":\"$.roles[0].id\",\"name\":\"$.roles[0].name\"},\"phone1\":\"$.phone[0]\",\"phone2\":\"$.phone[1]\",\"suggests\":[\"$.define.suggest.message\"],\"suggest1\":\"$.define.suggest.message[0]\",\"suggest2\":\"$.define.suggest.message[1]\",\"suggest3\":\"$.define.suggest.message[2]\"}";

        JToken jToken = JToken.Parse(jsonTemplate);

        if (jToken.Type != JTokenType.Object)
        {
            throw new ArgumentException("json type is not supported");
        }

        List<JsonPathMapperRelation> jsonPathMapperRelations = MapperObject(jToken);

        Assert.True(jsonPathMapperRelations.Count == 12);
    }

    private List<JsonPathMapperRelation> MapperObject(JToken jToken)
    {
        List<JsonPathMapperRelation> jsonPathMapperRelations = new List<JsonPathMapperRelation>();
        foreach (var jTokenProperty in jToken)
        {
            var path = jTokenProperty.Path;
            var obj = jToken.SelectToken(path);
            if (obj == null)
            {
                continue;
            }

            JsonPathMapperRelation jsonPathMapperRelation = new JsonPathMapperRelation();
            jsonPathMapperRelation.DestinationJsonColumnCode = path;

            if (obj.Type == JTokenType.String)
            {
                var sourceJsonPath = obj.Value<string>();

                jsonPathMapperRelation.DestinationPropertyType = DestinationPropertyTypeEnum.Property;
                jsonPathMapperRelation.SourceJsonPath = sourceJsonPath;
            }
            else if (obj.Type == JTokenType.Object)
            {
                jsonPathMapperRelation.DestinationPropertyType = DestinationPropertyTypeEnum.Object;
                jsonPathMapperRelation.SourceJsonPath = "$";

                var jsonString = obj.ToString();

                var jsonStringToken = JToken.Parse(jsonString);

                jsonPathMapperRelation.ChildRelations = MapperObject(jsonStringToken);
            }
            else if (obj.Type == JTokenType.Array)
            {
                jsonPathMapperRelation.DestinationPropertyType = DestinationPropertyTypeEnum.Array;

                var first = obj.First;
                if (first == null)
                {
                    continue;
                }

                if (first.Type == JTokenType.String)
                {
                    jsonPathMapperRelation.SourceJsonPath = first.Value<string>();
                }
                else if (first.Type == JTokenType.Object)
                {
                    jsonPathMapperRelation.SourceJsonPath = "$";

                    var jsonString = first.ToString();

                    var jsonStringToken = JToken.Parse(jsonString);

                    jsonPathMapperRelation.ChildRelations = MapperObject(jsonStringToken);
                }
                else
                {
                    continue;
                }
            }

            jsonPathMapperRelations.Add(jsonPathMapperRelation);
        }

        return jsonPathMapperRelations;
    }
}