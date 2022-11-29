namespace JsonPathConverter.Abstractions
{
    public interface IJsonColumnMapper
    {
        JsonMapperArray MapToCollection(string jsonSourceStr, JsonPathRoot jsonPathRoot);
        
        JsonMapperObject MapToObjectByTemplate(string jsonTemplate,string jsonSourceStr);

        TData? CaptureObject<TData>(string jsonSourceStr, string path);

        JsonMapperArray MapToCollectionByTemplate(string jsonTemplate, string jsonSourceStr);
    }
}
