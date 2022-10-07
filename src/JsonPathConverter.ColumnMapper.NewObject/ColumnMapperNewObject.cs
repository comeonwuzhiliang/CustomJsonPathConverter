using JsonPathConverter.Abstractions;
using JsonPathConverter.Newtonsoft.Helper;

namespace JsonPathConverter.ColumnMapper.NewObject
{
    public class ColumnMapperNewObject : IJsonColumnMapper
    {
        public JsonMapperArray MapToCollection(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            return new GenerateNewObject().MapArray(jsonSourceStr, jsonPathRoot) ?? new JsonMapperArray { };
        }

        public TData? CaptureObject<TData>(string jsonSourceStr, string path)
        {
            return new CaptureObject().Capture<TData>(jsonSourceStr, path);
        }
    }
}
