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
            JArray? jArray = new GenerateNewObject().MapArray(jsonSourceStr, jsonPathRoot);

            return jArray?.ToObject<IEnumerable<IDictionary<string, object?>>>() ?? new List<IDictionary<string, object?>>();
        }

        public IDictionary<string, object?> MapToDic(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            object? obj = new GenerateNewObject().Map(jsonSourceStr, jsonPathRoot);

            if (obj == null)
            {
                return new Dictionary<string, object?>();
            }

            if (obj is JObject)
            {
                return (obj as JObject)?.ToObject<IDictionary<string, object?>>() ?? new Dictionary<string, object?>();
            }

            if (obj is JArray)
            {
                JArray? jArray = obj as JArray;

                if (jArray?.Any() == true && jArray[0] != null)
                {
                    return jArray[0].ToObject<IDictionary<string, object?>>() ?? new Dictionary<string, object?>();
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
