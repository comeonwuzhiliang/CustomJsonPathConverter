using Newtonsoft.Json.Linq;

namespace JsonPathConverter.ColumnMapper.NewObject
{
    internal class MapperArray
    {
        public IEnumerable<IDictionary<string, object?>>? Array { get; set; }

        public JArray? JArray { get; set; }
    }
}
