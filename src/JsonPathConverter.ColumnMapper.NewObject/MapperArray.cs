using Newtonsoft.Json.Linq;

namespace JsonPathConverter.ColumnMapper.NewObject
{
    internal class MapperArray
    {
        public List<IDictionary<string, object?>>? Array { get; set; } = null;

        public JArray? JArray { get; set; } = null;

        public void Init()
        {
            JArray ??= new JArray();
            Array ??= new List<IDictionary<string, object?>>();
        }
    }
}
