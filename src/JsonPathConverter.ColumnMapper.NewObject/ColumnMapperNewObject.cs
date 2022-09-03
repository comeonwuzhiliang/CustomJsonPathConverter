using JsonPathConverter.Abstractions;
using System.Text.Json;
using static System.Text.Json.JsonElement;

namespace JsonPathConverter.ColumnMapper.SystemText
{
    public class ColumnMapperNewObject : IJsonColumnMapper
    {
        //public List<Dictionary<string, object?>> MapToCollection(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        //{
        //    JsonDocument? jsonSourceDocument = JsonSerializer.Deserialize<JsonDocument>(jsonSourceStr);

        //    JsonElement rootJsonElement = jsonSourceDocument?.RootElement ?? default;

        //    List<JsonElementRelation> sourceJsonElementRelations = new List<JsonElementRelation>();

        //    Dictionary<string, JsonElementDetail> sourceColumnJsonElements = new Dictionary<string, JsonElementDetail>();

        //    string rootPath = jsonPathRoot.RootPath ?? "$";

        //    // iterate destination json column
        //    foreach (var destinationJsonColumn in jsonPathRoot.DestinationJsonColumns ?? new List<DestinationJsonColumn>())
        //    {
        //        // TODO: 使用JsonElement的ObjectEnumerator内置对象的Current的Name进行判断（来实现忽略大小写的功能）
        //        var destinationMapperColumn = jsonPathRoot.JsonPathMapperRelations?.FirstOrDefault(s => s.DestinationJsonColumnCode == destinationJsonColumn.Code);

        //        string sourceJsonPath = string.Empty;

        //        if (destinationMapperColumn != null)
        //        {
        //            sourceJsonPath = destinationMapperColumn.SourceJsonPath ?? string.Empty;
        //        }
        //        else
        //        {
        //            // find same as destination column name
        //            sourceJsonPath = $"{rootPath}.{destinationJsonColumn!.Code}";
        //        }

        //        JsonElementDetail jsonElementDetail = new JsonElementDetail();

        //        // recursion
        //        // TODO: 需要减少结构体的复制，使用ref引用
        //        sourceJsonElementRelations = sourceJsonPath.Split(".").Aggregate(new List<JsonElementRelation>(), (jes, path) =>
        //        {
        //            var sourceJsonColumnElements = new List<JsonElement>();

        //            var sourceJsonColumnElementRelations = new List<JsonElementRelation>();

        //            if (path == "$")
        //            {
        //                sourceJsonColumnElementRelations.Add(new JsonElementRelation { Self = rootJsonElement });
        //                return sourceJsonColumnElementRelations;
        //            }

        //            bool isRecordAncestors = false;

        //            if (!rootPath.Contains($".{path}.") && !rootPath.EndsWith($".{path}"))
        //            {
        //                isRecordAncestors = true;
        //            }

        //            foreach (var je in jes)
        //            {
        //                try
        //                {
        //                    if (je.Self.ValueKind == JsonValueKind.Array)
        //                    {
        //                        Guid guid = Guid.NewGuid();
        //                        bool isArray = false;
        //                        ArrayEnumerator jsonElements = je.Self.EnumerateArray();
        //                        foreach (var jsonElement in jsonElements)
        //                        {
        //                            var rootObjectJsonElement = jsonElement;
        //                            if (je.Self.ValueKind == JsonValueKind.Array)
        //                            {
        //                                if (je.Ancestors.Any())
        //                                {
        //                                    isArray = true;
        //                                }
        //                            }

        //                            var jsonElementRelation = new JsonElementRelation
        //                            {
        //                                ColumnName = destinationJsonColumn.Code!,
        //                                ArrayId = guid,
        //                                IsArray = isArray,
        //                                Self = jsonElement.GetProperty(path),
        //                                Ancestors = isRecordAncestors ? new List<JsonElement>(je.Ancestors) { je.Self, jsonElement } : new List<JsonElement>()
        //                            };
        //                            sourceJsonColumnElementRelations.Add(jsonElementRelation);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        var jsonElementRelation = new JsonElementRelation
        //                        {
        //                            ColumnName = destinationJsonColumn.Code!,
        //                            Self = je.Self.GetProperty(path),
        //                            Ancestors = isRecordAncestors ? new List<JsonElement>(je.Ancestors) { je.Self } :
        //                            new List<JsonElement>()
        //                        };
        //                        sourceJsonColumnElementRelations.Add(jsonElementRelation);
        //                    }
        //                }
        //                catch
        //                {
        //                    sourceJsonColumnElements.Clear();
        //                    break;
        //                }
        //            }

        //            return sourceJsonColumnElementRelations;

        //        });

        //        jsonElementDetail.JsonElementRelations = sourceJsonElementRelations;
        //        sourceColumnJsonElements.Add(destinationJsonColumn.Code!, jsonElementDetail);
        //    }

        //    return GenerateDestinationJsonDic(sourceColumnJsonElements);
        //}

        //private List<Dictionary<string, object?>> GenerateDestinationJsonDic(Dictionary<string, JsonElementDetail> jsonSourceElements)
        //{
        //    var propertyNames = jsonSourceElements.Select(s => s.Key);

        //    // TODO: 字典类型使用JsonElement类型
        //    List<Dictionary<string, object?>> destinationJsonCollection = new List<Dictionary<string, object?>>();

        //    var groupsByRoot = jsonSourceElements.SelectMany(s => s.Value.JsonElementRelations).GroupBy(s => s.RootObjectJsonElement);

        //    foreach (var groupRoots in groupsByRoot)
        //    {
        //        Dictionary<string, object?> destinationJsonDic = new Dictionary<string, object?>();
        //        foreach (var group in groupRoots.GroupBy(s => s.ColumnName))
        //        {
        //            var firstItem = group.First();
        //            if (firstItem.IsArray == false)
        //            {
        //                destinationJsonDic[firstItem.ColumnName] = firstItem.Self;
        //            }
        //            else
        //            {
        //                destinationJsonDic[firstItem.ColumnName] = new List<object?>();
        //                foreach (var item in group)
        //                {
        //                    (destinationJsonDic[firstItem.ColumnName] as List<object?>)!.Add(item.Self);
        //                }
        //            }
        //        }
        //        destinationJsonCollection.Add(destinationJsonDic);
        //    }

        //    return destinationJsonCollection;
        //}

        //public Dictionary<string, object?> MapToDic(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        //{
        //    var list = MapToCollection(jsonSourceStr, jsonPathRoot);
        //    if (list.Any())
        //    {
        //        return list[0];
        //    }
        //    else
        //    {
        //        return new Dictionary<string, object?>();
        //    }
        //}
        public JsonMapResult<IEnumerable<IDictionary<string, object?>>> MapToCollection(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            throw new NotImplementedException();
        }

        public JsonMapResult<IDictionary<string, object?>> MapToDic(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            throw new NotImplementedException();
        }
    }
}
