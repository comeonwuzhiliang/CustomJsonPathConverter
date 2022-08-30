using JsonPathConverter.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonPathConverter.DefaultColumnMapper
{
    public class NewtonsoftJsonColumnMapper : IJsonColumnMapper
    {
        public List<Dictionary<string, object?>> MapToCollection(string jsonSourceStr, IEnumerable<DestinationJsonColumn>? destinationJsonColumns, IEnumerable<JsonPathMapperRelation>? jsonPathMapperRelations)
        {
            string destinationJson = MapToStr(jsonSourceStr, destinationJsonColumns, jsonPathMapperRelations);
            return JsonConvert.DeserializeObject<List<Dictionary<string, object?>>>(destinationJson);
        }

        public Dictionary<string, object?> MapToDic(string jsonSourceStr, IEnumerable<DestinationJsonColumn>? destinationJsonColumns, IEnumerable<JsonPathMapperRelation>? jsonPathMapperRelations)
        {
            string destinationJson = MapToStr(jsonSourceStr, destinationJsonColumns, jsonPathMapperRelations);
            return JsonConvert.DeserializeObject<Dictionary<string, object?>>(destinationJson);
        }

        private string MapToStr(string jsonSourceStr, IEnumerable<DestinationJsonColumn>? destinationJsonColumns, IEnumerable<JsonPathMapperRelation>? jsonPathMapperRelations)
        {
            var dic = jsonPathMapperRelations?.ToDictionary(s => s.SourceJsonPath ?? string.Empty) ?? new Dictionary<string, JsonPathMapperRelation>();
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
