using JsonPathConverter.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonPathConverter.ColumnMapper.NewObject;

internal class JsonTemplateConvertRelation
{
    internal List<JsonPathMapperRelation> ConvertRelations(string jsonTemplate)
    {
        JToken jToken = JToken.Parse(jsonTemplate);

        if (jToken.Type != JTokenType.Object)
        {
            throw new ArgumentException("json type is not supported");
        }

        List<JsonPathMapperRelation> jsonPathMapperRelations = MapperObject(jToken);

        return jsonPathMapperRelations;
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