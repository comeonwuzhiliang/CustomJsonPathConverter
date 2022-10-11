using JsonPathConverter.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using JsonPathConverter.Newtonsoft.Helper;

namespace JsonPathConverter.ColumnMapper.ReplaceKey
{
    public class ColumnMapperReplaceKey : IJsonColumnMapper
    {
        public JsonMapperArray MapToCollection(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            var map = MapToStr<IEnumerable<IDictionary<string, object?>>>(jsonSourceStr, jsonPathRoot);

            return new JsonMapperArray
                { Data = map.MapData ?? new List<IDictionary<string, object?>>(), Json = map.MapJsonStr };
        }

        public JsonMapperObject MapToObjectByTemplate(string jsonTemplate, string jsonSourceStr)
        {
            throw new Exception("not support");
        }

        public TData? CaptureObject<TData>(string jsonSourceStr, string path)
        {
            return new CaptureObject().Capture<TData>(jsonSourceStr, path);
        }

        private JsonMapResult<TData> MapToStr<TData>(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            var result = new JsonMapResult<TData>((str) => JsonConvert.DeserializeObject<TData>(str));

            var relations = jsonPathRoot.JsonPathMapperRelations?.ToList();
            if (relations == null || relations.Count == 0)
            {
                return result;
            }

            var jToken = JToken.Parse(jsonSourceStr);
            if (jToken == null)
            {
                return result;
            }

            if (!string.IsNullOrEmpty(jsonPathRoot.RootPath))
            {
                try
                {
                    jToken = jToken.SelectToken(jsonPathRoot.RootPath);
                    if (jToken != null && jToken.Type != JTokenType.Array
                                       && typeof(TData).IsGenericType
                                       && typeof(TData).IsAssignableTo(typeof(IEnumerable)))
                    {
                        result = new JsonMapResult<TData>((str) =>
                        {
                            var newStr = $"[{str}]";

                            return JsonConvert.DeserializeObject<TData>(newStr);
                        });
                    }
                }
                catch
                {
                    throw new Exception("json来源的数组项配置不正确");
                }
            }

            if (jToken == null)
            {
                return result;
            }

            JsonPathAdapter jsonPathAdapter = new JsonPathAdapter();

            foreach (var relation in relations)
            {
                string jsonPath = relation.SourceJsonPath ?? string.Empty;

                string jsonPathAdapterResult = jsonPathAdapter.Adapter(jsonPath, jToken);

                var matchJsonToekns = jToken.SelectTokens(jsonPathAdapterResult).ToList();
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