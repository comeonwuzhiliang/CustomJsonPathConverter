using JsonPathConverter.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonPathConverter.DefaultColumnMapper
{
    public class NewtonsoftJsonColumnMapper : IJsonColumnMapper
    {
        public List<Dictionary<string, object?>> MapToCollection(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            string destinationJson = MapToStr(jsonSourceStr, jsonPathRoot);
            return JsonConvert.DeserializeObject<List<Dictionary<string, object?>>>(destinationJson);
        }

        public Dictionary<string, object?> MapToDic(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            string destinationJson = MapToStr(jsonSourceStr, jsonPathRoot);
            return JsonConvert.DeserializeObject<Dictionary<string, object?>>(destinationJson);
        }

        private string MapToStr(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            var dic = jsonPathRoot.JsonPathMapperRelations?.ToDictionary(s => s.SourceJsonPath ?? string.Empty) ?? new Dictionary<string, JsonPathMapperRelation>();
            var jObj = JObject.Parse(jsonSourceStr);
            if (jObj == null)
            {
                return string.Empty;
            }
            var jObjCopy = jObj.DeepClone();
            var reader = jObj.CreateReader();
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                {
                    continue;
                }
                var matchPath = "$." + reader.Path;
                if (!dic.ContainsKey(matchPath))
                {
                    continue;
                }
                // 匹配json属性的value
                var jVal = jObjCopy.SelectToken(reader.Path);
                // 匹配的json属性
                var jProperty = jVal.Parent;
                // 用DestinationJsonColumnCode和jval构建新的属性
                var newProperty = new JProperty(dic[matchPath].DestinationJsonColumnCode, jVal);
                // 替换旧属性
                jProperty.Replace(newProperty);
            }
            return jObjCopy.ToString();
        }
    }
}
