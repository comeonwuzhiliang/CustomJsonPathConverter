namespace JsonPathConverter.Abstractions
{
    public interface IJsonDataProvider
    {
        Task<string> GetJsonDataAsync(IJsonRequestSource jsonRequestSource, CancellationToken cancellationToken = default);

        Task GetNoJsonDataAsync(IJsonRequestSource jsonRequestSource, CancellationToken cancellationToken = default);
    }
}
