namespace JsonPathConverter.JsonSource.HttpApi.Oauth
{
    public class TokenService : ITokenService
    {
        private readonly Func<CancellationToken, Task<string>> _tokenInvoker;

        public TokenService(Func<CancellationToken, Task<string>> tokenInvoker)
        {
            _tokenInvoker = tokenInvoker;
        }

        public async Task<string> GetToken(CancellationToken cancellationToken = default)
        {
            return await _tokenInvoker.Invoke(cancellationToken);
        }
    }
}
