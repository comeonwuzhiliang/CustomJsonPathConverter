using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonPathConverter.DefaultColumnMapper
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

        public JsonElement HostObjectJsonElement { get; set; }

        public JsonElement Self { get; set; }

        public List<JsonElement> Ancestors { get; set; } = new List<JsonElement>();
    }
}
