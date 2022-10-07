namespace JsonPathConverter.Abstractions
{
    public interface IJsonColumnMapper
    {
        JsonMapperArray MapToCollection(string jsonSourceStr, JsonPathRoot jsonPathRoot);

        TData? CaptureObject<TData>(string jsonSourceStr, string path);
    }
}
