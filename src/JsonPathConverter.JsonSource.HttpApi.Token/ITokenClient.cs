using IdentityModel.Client;

namespace JsonPathConverter.JsonSource.HttpApi.Oauth
{
    public interface ITokenClient<TRequest> where TRequest : TokenRequest
    {
        Task<string> GetAccessTokenAsync(TRequest? request, CancellationToken cancellationToken = default);
    }
}
