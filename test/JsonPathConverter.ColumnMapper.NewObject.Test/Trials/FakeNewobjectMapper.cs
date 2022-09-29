using JsonPathConverter.Abstractions;
using JsonPathConverter.Newtonsoft.Helper;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonPathConverter.ColumnMapper.NewObject.Test.Trials
{
    public class FakeNewobjectMapper
    {
        [Fact]
        public void GenerateOneLayerObject()
        {
            string json = "{\"name\":\"zhangsan\",\"age\":27}";

            JsonPathRoot jsonPathRoot = new JsonPathRoot("$");

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "Name", SourceJsonPath = "$.name" });

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "Age", SourceJsonPath = "$.age" });

            var mapResult = Map(json, jsonPathRoot);

            Assert.True(mapResult is JObject);

            var jObjectToDic = (mapResult as JObject)!.ToObject<IDictionary<string, object>>();

            Assert.Equal("zhangsan", jObjectToDic!["Name"]);

            Assert.Equal("27", jObjectToDic!["Age"].ToString());
        }

        [Fact]
        public void GenerateTwoLayerObject()
        {
            string json = "{\"name\":\"zhangsan\",\"address\":{\"province\":\"江苏省\",\"city\":\"盐城市\",\"county\":\"盐都区\"}}";

            JsonPathRoot jsonPathRoot = new JsonPathRoot("$");

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "Name", SourceJsonPath = "$.name" });

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation
            {
                DestinationJsonColumnCode = "Address",
                DestinationPropertyType = DestinationPropertyTypeEnum.Object,
                ChildRelations = new List<JsonPathMapperRelation> {
                    new JsonPathMapperRelation { DestinationJsonColumnCode = "Province", SourceJsonPath ="$.address.province"  },
                    new JsonPathMapperRelation { DestinationJsonColumnCode = "City", SourceJsonPath ="$.address.city"  },
                    new JsonPathMapperRelation { DestinationJsonColumnCode = "County", SourceJsonPath ="$.address.county"  }
                }
            });

            var mapResult = Map(json, jsonPathRoot);

            Assert.True(mapResult is JObject);

            var jObjectToDic = (mapResult as JObject)!.ToObject<IDictionary<string, object>>();

            Assert.Equal("zhangsan", jObjectToDic!["Name"]);

            var jObjectAddressToDic = (jObjectToDic["Address"] as JObject)!.ToObject<IDictionary<string, object>>();

            Assert.Equal("江苏省", jObjectAddressToDic!["Province"]!.ToString());

            Assert.Equal("盐城市", jObjectAddressToDic!["City"]!.ToString());

            Assert.Equal("盐都区", jObjectAddressToDic!["County"]!.ToString());
        }

        [Fact]
        public void GenerateTwoLayerObjectArray()
        {
            string json = "{\"name\":\"zhangsan\",\"address\":{\"province\":\"江苏省\",\"city\":\"盐城市\",\"county\":\"盐都区\"}}";

            JsonPathRoot jsonPathRoot = new JsonPathRoot("$");

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "Name", SourceJsonPath = "$.name" });

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation
            {
                DestinationJsonColumnCode = "Address",
                DestinationPropertyType = DestinationPropertyTypeEnum.Object,
                ChildRelations = new List<JsonPathMapperRelation> {
                    new JsonPathMapperRelation { DestinationJsonColumnCode = "Province", SourceJsonPath ="$.address.province"  },
                    new JsonPathMapperRelation { DestinationJsonColumnCode = "City", SourceJsonPath ="$.address.city"  },
                    new JsonPathMapperRelation { DestinationJsonColumnCode = "County", SourceJsonPath ="$.address.county"  }
                }
            });

            var mapResult = MapArrary(JToken.Parse(json), jsonPathRoot.JsonPathMapperRelations, new JsonPathAdapter());

            Assert.True(mapResult is JArray);

            var jObjectToDic = mapResult![0]!.ToObject<IDictionary<string, object>>();

            Assert.Equal("zhangsan", jObjectToDic!["Name"]);

            var jObjectAddressToDic = (jObjectToDic["Address"] as JObject)!.ToObject<IDictionary<string, object>>();

            Assert.Equal("江苏省", jObjectAddressToDic!["Province"]!.ToString());

            Assert.Equal("盐城市", jObjectAddressToDic!["City"]!.ToString());

            Assert.Equal("盐都区", jObjectAddressToDic!["County"]!.ToString());
        }

        [Fact]
        public void GenerateNoLayerArrayObject()
        {
            string json = "[{\"name\":\"zhangsan\",\"age\":27},{\"name\":\"lisi\",\"age\":28}]";

            JsonPathRoot jsonPathRoot = new JsonPathRoot("$");

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "Name", SourceJsonPath = "$.name" });

            jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "Age", SourceJsonPath = "$.age" });

            var mapResult = Map(json, jsonPathRoot);

            Assert.True(mapResult is JArray);

            var jObjectToDic1 = (mapResult as JArray)![0]!.ToObject<IDictionary<string, object>>();

            Assert.Equal("zhangsan", jObjectToDic1!["Name"]);

            Assert.Equal("27", jObjectToDic1!["Age"].ToString());

            var jObjectToDic2 = (mapResult as JArray)![1]!.ToObject<IDictionary<string, object>>();

            Assert.Equal("lisi", jObjectToDic2!["Name"]);

            Assert.Equal("28", jObjectToDic2!["Age"].ToString());
        }

        private object? Map(string jsonSourceStr, JsonPathRoot jsonPathRoot)
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
    }
}
