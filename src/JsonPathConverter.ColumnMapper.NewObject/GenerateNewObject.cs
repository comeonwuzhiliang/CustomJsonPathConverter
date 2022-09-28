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

            JArray jArray = new JArray();

            if (jToken.Type == JTokenType.Object)
            {
                return MapClass(jToken, jsonPathRoot.JsonPathMapperRelations, jsonPathAdapter);
            }

            if (jToken.Type == JTokenType.Array)
            {
                return MapArrary(jToken, jsonPathRoot.JsonPathMapperRelations, jsonPathAdapter);
            }

            return jArray;
        }

        private JObject? MapClass(JToken jToken, IEnumerable<JsonPathMapperRelation> relations, JsonPathAdapter jsonPathAdapter)
        {
            //if (relations?.Any(s => s.SourceJsonPath == "$.interalDepartment.id") == true)
            //{

            //}

            JObject jObject = new JObject();
            foreach (var relation in relations)
            {
                if (string.IsNullOrEmpty(relation.DestinationJsonColumnCode))
                {
                    return null;
                }

                string jsonPath = relation.SourceJsonPath ?? string.Empty;

                string jsonPathAdapterResult = jsonPathAdapter.Adapter(jsonPath, jToken);

                try
                {
                    var value = jToken.SelectToken(jsonPathAdapterResult);
                    if (value == null)
                    {
                        jObject.Add(relation.DestinationJsonColumnCode, value);
                        continue;
                    }

                    if (relation.DestinationPropertyType == DestinationPropertyTypeEnum.Property)
                    {
                        jObject.Add(relation.DestinationJsonColumnCode, value);
                        continue;
                    }

                    if (relation.ChildRelations?.Any() == false)
                    {
                        jObject.Add(relation.DestinationJsonColumnCode, null);
                    }

                    if (relation.DestinationPropertyType == DestinationPropertyTypeEnum.Object)
                    {
                        var classJObject = MapClass(value, relation.ChildRelations!, jsonPathAdapter);
                        jObject.Add(relation.DestinationJsonColumnCode, classJObject);
                    }
                    else if (relation.DestinationPropertyType == DestinationPropertyTypeEnum.Array)
                    {
                        var arrayJObject = MapArrary(value, relation.ChildRelations!, jsonPathAdapter);
                        jObject.Add(relation.DestinationJsonColumnCode, arrayJObject);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return jObject;
        }

        private JArray? MapArrary(JToken jToken, IEnumerable<JsonPathMapperRelation> relations, JsonPathAdapter jsonPathAdapter)
        {
            JArray jArray = new JArray();

            if (jToken.Type == JTokenType.Array)
            {
                foreach (var jTokenItem in jToken)
                {
                    JObject? jObject = MapClass(jTokenItem, relations, jsonPathAdapter);

                    if (jObject != null)
                    {
                        jArray.Add(jObject);
                    }
                }
            }
            else if (jToken.Type == JTokenType.Object)
            {
                JObject? jObject = MapClass(jToken, relations, jsonPathAdapter);
                if (jObject != null)
                {
                    jArray.Add(jObject);
                }
            }

            return jArray;
        }

        public JArray? MapArray(string jsonSourceStr, JsonPathRoot jsonPathRoot)
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

            JArray? jArray = MapArrary(jToken, jsonPathRoot.JsonPathMapperRelations, new JsonPathAdapter());

            return jArray;
        }
    }
}
