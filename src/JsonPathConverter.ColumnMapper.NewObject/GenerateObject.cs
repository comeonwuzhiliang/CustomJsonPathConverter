using JsonPathConverter.Abstractions;
using System.Text.Json;
using static System.Text.Json.JsonElement;

namespace JsonPathConverter.ColumnMapper.NewObject
{
    internal class GenerateObject
    {
        internal static List<IDictionary<string, object?>> MapToList(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            JsonDocument? jsonSourceDocument = JsonSerializer.Deserialize<JsonDocument>(jsonSourceStr);

            if (jsonSourceDocument != null && jsonPathRoot.RootPath != null)
            {
                JsonElement jsonElement = jsonSourceDocument?.RootElement ?? default;
                foreach (var item in jsonPathRoot.RootPath.Split("."))
                {
                    if (item == "$")
                    {
                        continue;
                    }
                    jsonElement = jsonElement.GetProperty(item);
                }

                var newJsonStr = JsonSerializer.Serialize(jsonElement);

                jsonSourceDocument = JsonSerializer.Deserialize<JsonDocument>(newJsonStr);
            }

            JsonElement rootJsonElement = jsonSourceDocument?.RootElement ?? default;

            List<JsonElementRelation> sourceJsonElementRelations = new List<JsonElementRelation>();

            Dictionary<string, JsonElementDetail> sourceColumnJsonElements = new Dictionary<string, JsonElementDetail>();

            // iterate destination json column
            foreach (var destinationJsonColumn in jsonPathRoot.DestinationJsonColumns ?? new List<DestinationJsonColumn>())
            {
                // TODO: 使用JsonElement的ObjectEnumerator内置对象的Current的Name进行判断（来实现忽略大小写的功能）
                var destinationMapperColumn = jsonPathRoot.JsonPathMapperRelations?.FirstOrDefault(s => s.DestinationJsonColumnCode == destinationJsonColumn.Code);

                string sourceJsonPath = string.Empty;

                if (destinationMapperColumn != null)
                {
                    sourceJsonPath = destinationMapperColumn.SourceJsonPath ?? string.Empty;
                }
                else
                {
                    // find same as destination column name
                    sourceJsonPath = $"$.{destinationJsonColumn!.Code}";
                }

                JsonElementDetail jsonElementDetail = new JsonElementDetail();

                // recursion
                sourceJsonElementRelations = sourceJsonPath.Split(".").Aggregate(new List<JsonElementRelation>(), (jes, path) =>
                {
                    var sourceJsonColumnElements = new List<JsonElement>();

                    var sourceJsonColumnElementRelations = new List<JsonElementRelation>();

                    if (path == "$")
                    {
                        sourceJsonColumnElementRelations.Add(new JsonElementRelation { Self = rootJsonElement });
                        return sourceJsonColumnElementRelations;
                    }

                    foreach (var je in jes)
                    {
                        try
                        {
                            if (je.Self.ValueKind == JsonValueKind.Array)
                            {
                                Guid guid = Guid.NewGuid();
                                bool isArray = false;
                                ArrayEnumerator jsonElements = je.Self.EnumerateArray();
                                foreach (var jsonElement in jsonElements)
                                {
                                    var rootObjectJsonElement = jsonElement;
                                    if (je.Self.ValueKind == JsonValueKind.Array)
                                    {
                                        if (je.Ancestors.Any())
                                        {
                                            isArray = true;
                                        }
                                    }

                                    var jsonElementRelation = new JsonElementRelation
                                    {
                                        ColumnName = destinationJsonColumn.Code!,
                                        ArrayId = guid,
                                        IsArray = isArray,
                                        Self = jsonElement.GetProperty(path),
                                        Ancestors = new List<JsonElement>(je.Ancestors) { je.Self, jsonElement }
                                    };
                                    sourceJsonColumnElementRelations.Add(jsonElementRelation);
                                }
                            }
                            else
                            {
                                var jsonElementRelation = new JsonElementRelation
                                {
                                    ColumnName = destinationJsonColumn.Code!,
                                    Self = je.Self.GetProperty(path),
                                    Ancestors = new List<JsonElement>(je.Ancestors) { je.Self }
                                };
                                sourceJsonColumnElementRelations.Add(jsonElementRelation);
                            }
                        }
                        catch
                        {
                            sourceJsonColumnElements.Clear();
                            break;
                        }
                    }

                    return sourceJsonColumnElementRelations;

                });

                jsonElementDetail.JsonElementRelations = sourceJsonElementRelations;
                sourceColumnJsonElements.Add(destinationJsonColumn.Code!, jsonElementDetail);
            }

            return GenerateDestinationJsonDic(sourceColumnJsonElements);
        }

        private static List<IDictionary<string, object?>> GenerateDestinationJsonDic(Dictionary<string, JsonElementDetail> jsonSourceElements)
        {
            var propertyNames = jsonSourceElements.Select(s => s.Key);

            List<IDictionary<string, object?>> destinationJsonCollection = new List<IDictionary<string, object?>>();

            var groupsByRoot = jsonSourceElements.SelectMany(s => s.Value.JsonElementRelations).GroupBy(s => s.RootObjectJsonElement);

            foreach (var groupRoots in groupsByRoot)
            {
                IDictionary<string, object?> destinationJsonDic = new Dictionary<string, object?>();
                foreach (var group in groupRoots.GroupBy(s => s.ColumnName))
                {
                    var firstItem = group.First();
                    if (firstItem.IsArray == false)
                    {
                        destinationJsonDic[firstItem.ColumnName] = firstItem.Self;
                    }
                    else
                    {
                        destinationJsonDic[firstItem.ColumnName] = new List<object?>();
                        foreach (var item in group)
                        {
                            (destinationJsonDic[firstItem.ColumnName] as List<object?>)!.Add(item.Self);
                        }
                    }
                }
                destinationJsonCollection.Add(destinationJsonDic);
            }

            return destinationJsonCollection;
        }

        internal static IDictionary<string, object?> MapToObject(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            var list = MapToList(jsonSourceStr, jsonPathRoot);
            if (list.Any())
            {
                return list[0];
            }
            else
            {
                return new Dictionary<string, object?>();
            }
        }
    }
}
