using JsonPathConverter.Abstractions;
using JsonPathConverter.Newtonsoft.Helper;
using Newtonsoft.Json.Linq;

namespace JsonPathConverter.ColumnMapper.NewObject
{
    internal class GenerateNewObject
    {
        private readonly IEnumerable<JsonPropertyFormatFunction>? _jsonPropertyFormatFunctions;
        public GenerateNewObject(IEnumerable<JsonPropertyFormatFunction>? jsonPropertyFormatFunctions = null)
        {
            _jsonPropertyFormatFunctions = jsonPropertyFormatFunctions;
        }

        private MapperClass? MapClass(JToken jToken, IEnumerable<JsonPathMapperRelation> relations, JsonPathAdapter jsonPathAdapter)
        {
            Dictionary<string, object?> dicObj = new Dictionary<string, object?>();

            JObject jObject = new JObject();

            MapperArray mapperArray = new MapperArray();

            foreach (var relation in relations)
            {
                if (string.IsNullOrEmpty(relation.DestinationJsonColumnCode))
                {
                    continue;
                }

                string jsonPath = relation.SourceJsonPath ?? string.Empty;

                var jsonPropertyFormatFunction = _jsonPropertyFormatFunctions?.FirstOrDefault(s => s.FormatKey?.ToLower() == jsonPath?.ToLower());

                if (jsonPropertyFormatFunction != null && jsonPropertyFormatFunction.FormatFunction != null)
                {
                    object jsonPropertyValue = jsonPropertyFormatFunction.FormatFunction();

                    dicObj[relation.DestinationJsonColumnCode] = jsonPropertyValue;

                    var formatJToken = JToken.FromObject(jsonPropertyValue);

                    jObject.Add(relation.DestinationJsonColumnCode, formatJToken);

                    continue;
                }
                else if (!jsonPath.StartsWith("$"))
                {
                    dicObj[relation.DestinationJsonColumnCode] = jsonPath;
                    jObject.Add(relation.DestinationJsonColumnCode, jsonPath);
                    continue;
                }

                string jsonPathAdapterResult = jsonPathAdapter.Adapter(jsonPath, jToken);

                try
                {
                    JToken? value = null;
                    IEnumerable<JToken>? values = null;
                    try
                    {
                        value = jToken.SelectToken(jsonPathAdapterResult);
                        if (value == null)
                        {
                            dicObj[relation.DestinationJsonColumnCode] = null;

                            jObject.Add(relation.DestinationJsonColumnCode, null);

                            continue;
                        }
                    }
                    catch
                    {
                        values = jToken.SelectTokens(jsonPathAdapterResult);
                    }

                    if (relation.DestinationPropertyType == DestinationPropertyTypeEnum.Property)
                    {
                        if (value != null && value is JValue)
                        {
                            try
                            {
                                JValue jValue = (JValue)value;

                                dicObj[relation.DestinationJsonColumnCode] = jValue.Value;

                                jObject.Add(relation.DestinationJsonColumnCode, value);
                            }
                            catch { }

                            continue;
                        }
                        else if (values != null || (value != null && value is JArray))
                        {
                            try
                            {
                                mapperArray.Init();

                                IEnumerable<JToken> jTokens;
                                if (values != null)
                                {
                                    jTokens = values;
                                }
                                else
                                {
                                    jTokens = value!;
                                }

                                for (int i = 0; i < jTokens.Count(); i++)
                                {
                                    var jTokenItem = jTokens.ElementAt(i);
                                    if (mapperArray.JArray!.Count() > i)
                                    {
                                        var existJObject = mapperArray.JArray![i];

                                        existJObject[relation.DestinationJsonColumnCode] = jTokenItem;

                                        var existDict = mapperArray.Array!.ElementAt(i);

                                        JValue jValue2 = (JValue)jTokenItem;

                                        existDict[relation.DestinationJsonColumnCode] = jValue2.Value;
                                    }
                                    else
                                    {
                                        JObject itemObject = new JObject();

                                        itemObject[relation.DestinationJsonColumnCode] = jTokenItem;

                                        mapperArray.JArray?.Add(itemObject);

                                        JValue jValueNew = (JValue)jTokenItem;

                                        Dictionary<string, object?> dicNew = new Dictionary<string, object?>();

                                        dicNew[relation.DestinationJsonColumnCode] = jValueNew.Value;

                                        mapperArray.Array!.Add(dicNew);
                                    }
                                }
                            }
                            catch { }

                            continue;
                        }
                        else
                        {
                            dicObj[relation.DestinationJsonColumnCode] = null;
                            jObject.Add(relation.DestinationJsonColumnCode, null);
                            continue;
                        }
                    }

                    if (relation.ChildRelations == null || relation.ChildRelations.Any() == false)
                    {
                        if (relation.DestinationPropertyType != DestinationPropertyTypeEnum.Array)
                        {
                            dicObj[relation.DestinationJsonColumnCode] = null;

                            jObject.Add(relation.DestinationJsonColumnCode, null);
                        }
                        else
                        {
                            JArray jArray = new JArray();
                            List<object?> list = new List<object?>();

                            if (values == null && value != null)
                            {
                                values = new List<JToken>() { value };
                            }

                            foreach (var item in values ?? new List<JToken>())
                            {
                                jArray.Add(item);
                                try
                                {
                                    JValue jValue = (JValue)item;

                                    list.Add(jValue.Value);
                                }
                                catch { }
                            }

                            jObject.Add(relation.DestinationJsonColumnCode, jArray);
                            dicObj[relation.DestinationJsonColumnCode] = list;
                        }
                    }
                    else
                    {
                        if (relation.DestinationPropertyType == DestinationPropertyTypeEnum.Object)
                        {
                            var dic = MapClass(value!, relation.ChildRelations!, jsonPathAdapter);

                            dicObj[relation.DestinationJsonColumnCode] = dic?.Object;

                            jObject.Add(relation.DestinationJsonColumnCode, dic?.JObject);
                        }
                        else if (relation.DestinationPropertyType == DestinationPropertyTypeEnum.Array)
                        {
                            var array = MapArrary(value!, relation.ChildRelations!, jsonPathAdapter);

                            dicObj[relation.DestinationJsonColumnCode] = array?.Array;

                            jObject.Add(relation.DestinationJsonColumnCode, array?.JArray);
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }

            if (mapperArray.JArray?.Any() == true)
            {
                foreach (var item in jObject)
                {
                    foreach (var arrayItem in mapperArray.JArray)
                    {
                        arrayItem[item.Key] = item.Value;
                    }
                }
            }

            if (mapperArray.Array?.Any() == true)
            {
                foreach (var item in dicObj)
                {
                    foreach (var arrayItem in mapperArray.Array)
                    {
                        arrayItem[item.Key] = item.Value;
                    }
                }
            }

            return new MapperClass { Object = dicObj, JObject = jObject, Array = mapperArray };
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

                MapperArray? mapperArray = mapperClass?.Array;

                if (mapperArray?.Array?.Any() == true)
                {
                    return mapperArray;
                }
                else
                {
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

            return new MapperArray { JArray = jArray, Array = list };
        }

        internal JsonMapperArray? MapArray(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            var relations = jsonPathRoot.JsonPathMapperRelations;

            if (relations.Count == 0)
            {
                return null;
            }

            var jToken = JToken.Parse(jsonSourceStr);

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

        internal JsonMapperObject? MapperObject(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            var relations = jsonPathRoot.JsonPathMapperRelations;

            if (relations.Count == 0)
            {
                return null;
            }

            var jToken = JToken.Parse(jsonSourceStr);

            if (!string.IsNullOrEmpty(jsonPathRoot.RootPath))
            {
                jToken = jToken.SelectToken(jsonPathRoot.RootPath);

                if (jToken == null)
                {
                    return null;
                }
            }

            var mapObject = MapClass(jToken, jsonPathRoot.JsonPathMapperRelations, new JsonPathAdapter());

            return new JsonMapperObject { Data = mapObject?.Object, Json = mapObject?.JObject?.ToString() };
        }
    }
}
