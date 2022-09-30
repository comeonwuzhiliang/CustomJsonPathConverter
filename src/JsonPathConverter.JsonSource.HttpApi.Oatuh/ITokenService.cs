namespace JsonPathConverter.JsonSource.HttpApi.Oauth
{
    public interface ITokenService
    {
        Task<string> GetToken(CancellationToken cancellationToken = default);
    }
}
