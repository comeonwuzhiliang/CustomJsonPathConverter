using JsonPathConverter.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonPathConverter.ColumnMapper.ReplaceKey
{
    public class ColumnMapperReplaceKey : IJsonColumnMapper
    {
        public JsonMapResult<IEnumerable<IDictionary<string, object?>>> MapToCollection(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {

            return MapToStr<IEnumerable<IDictionary<string, object?>>>(jsonSourceStr, jsonPathRoot);
        }

        public JsonMapResult<IDictionary<string, object?>> MapToDic(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {

            return MapToStr<IDictionary<string, object?>>(jsonSourceStr, jsonPathRoot);
        }

        private JsonMapResult<TData> MapToStr<TData>(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            var result = new JsonMapResult<TData>((str) => JsonConvert.DeserializeObject<TData>(str));

            var relations = jsonPathRoot.JsonPathMapperRelations?.Where(s => s.IsValidate()).OrderByDescending(s => s.GetFileds().Length).ToList();
            if (relations == null || relations.Count == 0)
            {
                return result;
            }
            var jToken = JToken.Parse(jsonSourceStr);
            if (jToken == null)
            {
                return result;
            }

            foreach (var relation in relations)
            {
                var matchJsonToekns = jToken.SelectTokens(relation.SourceJsonPath ?? string.Empty).ToList();
                if (matchJsonToekns == null || matchJsonToekns.Count() == 0)
                {
                    result.PropertyInfos.Add(new JsonMapInfo
                    {
                        SourcePath = relation.SourceJsonPath ?? string.Empty,
                        DestinationFiled = relation.DestinationJsonColumnCode ?? string.Empty,
                        ErrorMessage = "can't find transfer JTokens in json"
                    });
                    continue;
                }
                foreach (var token in matchJsonToekns)
                {
                    var jProperty = token.Parent;
                    if (jProperty == null)
                    {
                        continue;
                    }
                    if (jProperty.Type != JTokenType.Property)
                    {
                        result.PropertyInfos.Add(new JsonMapInfo
                        {
                            SourcePath = token.Path,
                            DestinationFiled = relation.DestinationJsonColumnCode ?? string.Empty,
                            ErrorMessage = "resolve type error"
                        });
                        continue;
                    }
                    var newProperty = new JProperty(relation.DestinationJsonColumnCode ?? string.Empty, token);
                    jProperty.Replace(newProperty);
                }
            }
            result.MapJsonStr = jToken.ToString();
            return result;
        }
    }
}
