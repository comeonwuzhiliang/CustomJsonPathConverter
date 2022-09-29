using JsonPathConverter.Abstractions;
using JsonPathConverter.Newtonsoft.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace JsonPathConverter.ColumnMapper.NewObject
{
    public class ColumnMapperNewObject : IJsonColumnMapper
    {
        public IEnumerable<IDictionary<string, object?>> MapToCollection(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            return new GenerateNewObject().MapArray(jsonSourceStr, jsonPathRoot) ?? new List<IDictionary<string, object?>>();
        }

        public IDictionary<string, object?> MapToDic(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            object? obj = new GenerateNewObject().Map(jsonSourceStr, jsonPathRoot);

            if (obj == null)
            {
                return new Dictionary<string, object?>();
            }

            if (obj is IDictionary<string, object?>)
            {
                return (obj as IDictionary<string, object?>) ?? new Dictionary<string, object?>();
            }

            if (obj is IEnumerable<IDictionary<string, object?>>)
            {
                var list = obj as List<IDictionary<string, object?>>;

                if (list?.Any() == true && list[0] != null)
                {
                    return list[0] ?? new Dictionary<string, object?>();
                }
            }

            return new Dictionary<string, object?>();
        }

        public TData? CaptureObject<TData>(string jsonSourceStr, string path)
        {
            return new CaptureObject().Capture<TData>(jsonSourceStr, path);
        }
    }
}
