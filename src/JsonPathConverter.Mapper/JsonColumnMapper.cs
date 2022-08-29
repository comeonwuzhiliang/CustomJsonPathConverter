using JsonPathConverter.Interface;
using System.Text.Json;
using static System.Text.Json.JsonElement;

namespace JsonPathConverter.DefaultColumnMapper
{
    public class JsonColumnMapper
    {
        public static List<Dictionary<string, object?>> JsonSourceElements(
            string jsonSourceStr,
            IEnumerable<DestinationJsonColumn>? destinationJsonColumns,
            IEnumerable<JsonPathMapperRelation>? jsonPathMapperRelations
            )
        {
            JsonDocument? jsonSourceDocument = JsonSerializer.Deserialize<JsonDocument>(jsonSourceStr);

            JsonElement rootJsonElement = jsonSourceDocument?.RootElement ?? default;

            List<JsonElementRelation> sourceJsonElementRelations = new List<JsonElementRelation>();

            Dictionary<string, JsonElementDetail> sourceColumnJsonElements = new Dictionary<string, JsonElementDetail>();

            // iterate destination json column
            foreach (var destinationJsonColumn in destinationJsonColumns ?? new List<DestinationJsonColumn>())
            {
                // TODO: 使用JsonElement的ObjectEnumerator内置对象的Current的Name进行判断（来实现忽略大小写的功能）
                var destinationMapperColumn = jsonPathMapperRelations?.FirstOrDefault(s => s.DestinationJsonColumnCode == destinationJsonColumn.Code);

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
                // TODO: 需要减少结构体的复制，使用ref引用
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
                                    var hostObjectJsonElement = jsonElement;
                                    if (je.Self.ValueKind == JsonValueKind.Array)
                                    {
                                        if (je.Ancestors.Any())
                                        {
                                            hostObjectJsonElement = je.Ancestors.Last();
                                            isArray = true;
                                        }
                                    }

                                    sourceJsonColumnElementRelations.Add(new JsonElementRelation
                                    {
                                        ColumnName = destinationJsonColumn.Code!,
                                        ArrayId = guid,
                                        IsArray = isArray,
                                        HostObjectJsonElement = hostObjectJsonElement,
                                        Self = jsonElement.GetProperty(path),
                                        Ancestors =
                                         new List<JsonElement>(je.Ancestors) { je.Self, jsonElement }
                                    });
                                }
                            }
                            else
                            {
                                sourceJsonColumnElementRelations.Add(new JsonElementRelation
                                {
                                    ColumnName = destinationJsonColumn.Code!,
                                    Self = je.Self.GetProperty(path),
                                    HostObjectJsonElement = je.Self,
                                    Ancestors =
                                         new List<JsonElement>(je.Ancestors) { je.Self }
                                });
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

        private static List<Dictionary<string, object?>> GenerateDestinationJsonDic(Dictionary<string, JsonElementDetail> jsonSourceElements)
        {
            var propertyNames = jsonSourceElements.Select(s => s.Key);

            // TODO: 字典类型使用JsonElement类型
            List<Dictionary<string, object?>> destinationJsonCollection = new List<Dictionary<string, object?>>();

            var groupsByHost = jsonSourceElements.SelectMany(s => s.Value.JsonElementRelations).GroupBy(s => s.HostObjectJsonElement);

            foreach (var groupHosts in groupsByHost)
            {
                Dictionary<string, object?> destinationJsonDic = new Dictionary<string, object?>();
                foreach (var group in groupHosts.GroupBy(s => s.ColumnName))
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

    }
}
