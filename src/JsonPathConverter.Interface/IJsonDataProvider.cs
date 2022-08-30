namespace JsonPathConverter.Interface
{
    public interface IJsonDataProvider
    {
        Task<List<Dictionary<string, object?>>> GetJsonData(JsonPathRoot jsonPathRoot, CancellationToken cancellationToken = default);
    }
}
