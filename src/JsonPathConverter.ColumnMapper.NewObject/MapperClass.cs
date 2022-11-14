using Newtonsoft.Json.Linq;

namespace JsonPathConverter.ColumnMapper.NewObject
{
    internal class MapperClass
    {
        public IDictionary<string, object?>? Object { get; set; }

        public JObject? JObject { get; set; }

        public MapperArray? Array { get; set; }
    }
}
