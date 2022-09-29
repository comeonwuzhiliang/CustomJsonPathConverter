using JsonPathConverter.Abstractions;
using JsonPathConverter.Newtonsoft.Helper;
using Newtonsoft.Json.Linq;

namespace JsonPathConverter.ColumnMapper.NewObject
{
    internal class GenerateNewObject
    {
        public object? Map(string jsonSourceStr, JsonPathRoot jsonPathRoot)
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

            JsonPathAdapter jsonPathAdapter = new JsonPathAdapter();

            if (jToken.Type == JTokenType.Object)
            {
                return MapClass(jToken, jsonPathRoot.JsonPathMapperRelations, jsonPathAdapter);
            }

            if (jToken.Type == JTokenType.Array)
            {
                return MapArrary(jToken, jsonPathRoot.JsonPathMapperRelations, jsonPathAdapter);
            }

            return null;
        }

        private IDictionary<string, object?>? MapClass(JToken jToken, IEnumerable<JsonPathMapperRelation> relations, JsonPathAdapter jsonPathAdapter)
        {
            Dictionary<string, object?> dicObj = new Dictionary<string, object?>();
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
                        continue;
                    }

                    if (relation.DestinationPropertyType == DestinationPropertyTypeEnum.Property)
                    {
                        JValue jValue = (JValue)value;

                        dicObj[relation.DestinationJsonColumnCode] = jValue.Value;

                        continue;
                    }

                    if (relation.ChildRelations?.Any() == false)
                    {
                        dicObj[relation.DestinationJsonColumnCode] = null;
                    }

                    if (relation.DestinationPropertyType == DestinationPropertyTypeEnum.Object)
                    {
                        var dic = MapClass(value, relation.ChildRelations!, jsonPathAdapter);
                        dicObj[relation.DestinationJsonColumnCode] = dic;
                    }
                    else if (relation.DestinationPropertyType == DestinationPropertyTypeEnum.Array)
                    {
                        var array = MapArrary(value, relation.ChildRelations!, jsonPathAdapter);
                        dicObj[relation.DestinationJsonColumnCode] = array;
                    }
                }
                catch
                {
                    continue;
                }
            }

            return dicObj;
        }

        private IEnumerable<IDictionary<string, object?>>? MapArrary(JToken jToken, IEnumerable<JsonPathMapperRelation> relations, JsonPathAdapter jsonPathAdapter)
        {
            List<IDictionary<string, object?>> list = new List<IDictionary<string, object?>>();

            if (jToken.Type == JTokenType.Array)
            {
                foreach (var jTokenItem in jToken)
                {
                    IDictionary<string, object?>? dic = MapClass(jTokenItem, relations, jsonPathAdapter);

                    if (dic != null)
                    {
                        list.Add(dic);
                    }
                }
            }
            else if (jToken.Type == JTokenType.Object)
            {
                IDictionary<string, object?>? dic = MapClass(jToken, relations, jsonPathAdapter);
                if (dic != null)
                {
                    list.Add(dic);
                }
            }

            return list;
        }

        public IEnumerable<IDictionary<string, object?>>? MapArray(string jsonSourceStr, JsonPathRoot jsonPathRoot)
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

            IEnumerable<IDictionary<string, object?>>? list = MapArrary(jToken, jsonPathRoot.JsonPathMapperRelations, new JsonPathAdapter());

            return list;
        }
    }
}
