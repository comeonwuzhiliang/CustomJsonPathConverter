namespace JsonPathConverter.Abstractions
{
    public interface IJsonColumnMapper
    {
        IEnumerable<IDictionary<string, object?>> MapToCollection(string jsonSourceStr, JsonPathRoot jsonPathRoot);

        IDictionary<string, object?> MapToDic(string jsonSourceStr, JsonPathRoot jsonPathRoot);

        TData? CaptureObject<TData>(string jsonSourceStr, string path);
    }
}
