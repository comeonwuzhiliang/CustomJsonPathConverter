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
        public bool IsArrary { get; set; }
        public List<JsonElementRelation> JsonElementRelations { get; set; } = new List<JsonElementRelation> { };
    }

    public class JsonElementRelation
    {
        public JsonElement Self { get; set; }

        public List<JsonElement> Ancestors { get; set; } = new List<JsonElement>();
    }
}
