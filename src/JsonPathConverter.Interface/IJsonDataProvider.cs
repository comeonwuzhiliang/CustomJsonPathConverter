namespace JsonPathConverter.Interface
{
    public interface IJsonDataProvider
    {
        Task<string> GetJsonData(JsonPathRoot jsonPathRoot, CancellationToken cancellationToken = default);
    }
}
