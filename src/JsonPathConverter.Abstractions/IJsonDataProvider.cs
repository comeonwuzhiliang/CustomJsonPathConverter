namespace JsonPathConverter.Abstractions
{
    public interface IJsonDataProvider
    {
        Task<IEnumerable<IDictionary<string, object?>>?> GetJsonData(JsonPathRoot jsonPathRoot, CancellationToken cancellationToken = default);
    }
}
