using JsonPathConverter.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Text.Json.JsonElement;

namespace JsonPathConverter.DefaultColumnMapper
{
    public class JsonColumnMapper
    {
        public static Dictionary<string, JsonElementDetail> JsonSourceElements(
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
                foreach (var path in sourceJsonPath.Split("."))
                {
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
                                    ArrayEnumerator jsonElements = je.Self.EnumerateArray();
                                    foreach (var jsonElement in jsonElements)
                                    {
                                        sourceJsonColumnElementRelations.Add(new JsonElementRelation
                                        {
                                            Self = jsonElement.GetProperty(path),
                                            Ancestors =
                                             new List<JsonElement>(je.Ancestors) { jsonElement }
                                        });
                                    }

                                    jsonElementDetail.IsArrary = true;
                                }
                                else
                                {
                                    sourceJsonColumnElementRelations.Add(new JsonElementRelation
                                    {
                                        Self = je.Self.GetProperty(path),
                                        Ancestors =
                                             new List<JsonElement>(je.Ancestors) { je.Self }
                                    });
                                    jsonElementDetail.IsArrary = false;
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
                }

                jsonElementDetail.JsonElementRelations = sourceJsonElementRelations;
                sourceColumnJsonElements.Add(destinationJsonColumn.Code!, jsonElementDetail);
            }

            return sourceColumnJsonElements;
        }

    }
}
