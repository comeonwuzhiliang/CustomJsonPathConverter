namespace JsonPathConverter.JsonSource.HttpApi.Token
{
    public interface ITokenService
    {
        Task<string> GetToken(CancellationToken cancellationToken = default);
    }
}
