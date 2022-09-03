using System.Text.Json;

namespace JsonPathConverter.ColumnMapper.NewObject
{
    public class JsonElementDetail
    {
        public List<JsonElementRelation> JsonElementRelations { get; internal set; } = new List<JsonElementRelation> { };
    }

    public class JsonElementRelation
    {
        public string ColumnName { get; set; } = string.Empty;

        public bool IsArray { get; set; }

        public Guid ArrayId { get; set; }

        public JsonElement RootObjectJsonElement
        {
            get
            {
                if (Ancestors?.Any() == true)
                {
                    if (Ancestors[0].ValueKind != JsonValueKind.Array)
                    {
                        return Ancestors[0];
                    }

                    if (Ancestors.Count == 1)
                    {
                        return Ancestors[0];
                    }

                    if (Ancestors.Count > 1)
                    {
                        return Ancestors[1];
                    }
                }

                return Self;
            }
        }

        public JsonElement Self { get; set; }

        public List<JsonElement> Ancestors { get; internal set; } = new List<JsonElement>();
    }
}
