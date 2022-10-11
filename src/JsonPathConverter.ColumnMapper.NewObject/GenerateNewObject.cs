using JsonPathConverter.Abstractions;
using JsonPathConverter.Newtonsoft.Helper;
using Newtonsoft.Json.Linq;

namespace JsonPathConverter.ColumnMapper.NewObject
{
    internal class GenerateNewObject
    {
        private MapperClass? MapClass(JToken jToken, IEnumerable<JsonPathMapperRelation> relations, JsonPathAdapter jsonPathAdapter)
        {
            Dictionary<string, object?> dicObj = new Dictionary<string, object?>();

            JObject jObject = new JObject();

            foreach (var relation in relations)
            {
                if (string.IsNullOrEmpty(relation.DestinationJsonColumnCode))
                {
                    continue;
                }

                string jsonPath = relation.SourceJsonPath ?? string.Empty;

                string jsonPathAdapterResult = jsonPathAdapter.Adapter(jsonPath, jToken);

                try
                {
                    var value = jToken.SelectToken(jsonPathAdapterResult);
                    if (value == null)
                    {
                        dicObj[relation.DestinationJsonColumnCode] = null;

                        jObject.Add(relation.DestinationJsonColumnCode, null);

                        continue;
                    }

                    if (relation.DestinationPropertyType == DestinationPropertyTypeEnum.Property)
                    {
                        try
                        {
                            JValue jValue = (JValue)value;

                            dicObj[relation.DestinationJsonColumnCode] = jValue.Value;
                        }
                        catch { }

                        jObject.Add(relation.DestinationJsonColumnCode, value);

                        continue;
                    }

                    if (relation.ChildRelations?.Any() == false)
                    {
                        dicObj[relation.DestinationJsonColumnCode] = null;

                        jObject.Add(relation.DestinationJsonColumnCode, null);
                    }

                    if (relation.DestinationPropertyType == DestinationPropertyTypeEnum.Object)
                    {
                        var dic = MapClass(value, relation.ChildRelations!, jsonPathAdapter);

                        dicObj[relation.DestinationJsonColumnCode] = dic?.Object;

                        jObject.Add(relation.DestinationJsonColumnCode, dic?.JObject);
                    }
                    else if (relation.DestinationPropertyType == DestinationPropertyTypeEnum.Array)
                    {
                        var array = MapArrary(value, relation.ChildRelations!, jsonPathAdapter);

                        dicObj[relation.DestinationJsonColumnCode] = array?.Array;

                        jObject.Add(relation.DestinationJsonColumnCode, array?.JArray);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return new MapperClass { Object = dicObj, JObject = jObject };
        }

        private MapperArray? MapArrary(JToken jToken, IEnumerable<JsonPathMapperRelation> relations, JsonPathAdapter jsonPathAdapter)
        {
            List<IDictionary<string, object?>> list = new List<IDictionary<string, object?>>();

            JArray jArray = new JArray();

            if (jToken.Type == JTokenType.Array)
            {
                foreach (var jTokenItem in jToken)
                {
                    MapperClass? mapperClass = MapClass(jTokenItem, relations, jsonPathAdapter);

                    IDictionary<string, object?>? dic = mapperClass?.Object;

                    JObject? jObject = mapperClass?.JObject;

                    if (dic != null)
                    {
                        list.Add(dic);
                    }

                    if (jObject != null)
                    {
                        jArray.Add(jObject);
                    }
                }
            }
            else if (jToken.Type == JTokenType.Object)
            {
                MapperClass? mapperClass = MapClass(jToken, relations, jsonPathAdapter);
                IDictionary<string, object?>? dic = mapperClass?.Object;

                JObject? jObject = mapperClass?.JObject;

                if (dic != null)
                {
                    list.Add(dic);
                }

                if (jObject != null)
                {
                    jArray.Add(jObject);
                }
            }

            return new MapperArray { JArray = jArray, Array = list };
        }

        public JsonMapperArray? MapArray(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            var relations = jsonPathRoot.JsonPathMapperRelations;

            if (relations == null || relations.Count == 0)
            {
                return null;
            }

            var jToken = JToken.Parse(jsonSourceStr);
            if (jToken == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(jsonPathRoot.RootPath))
            {
                jToken = jToken.SelectToken(jsonPathRoot.RootPath);

                if (jToken == null)
                {
                    return null;
                }
            }

            var mapArray = MapArrary(jToken, jsonPathRoot.JsonPathMapperRelations, new JsonPathAdapter());

            return new JsonMapperArray { Data = mapArray?.Array, Json = mapArray?.JArray?.ToString() };
        }
    }
}
