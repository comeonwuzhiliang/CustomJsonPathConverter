using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JsonPathConverter.JsonSource.HttpApi.Oauth
{
    public class AuthorizationCodeTokenClient : ITokenClient<AuthorizationCodeTokenRequest>
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly TokenClientOptions _options;
        public AuthorizationCodeTokenClient(
            HttpClient httpClient,
            IOptions<TokenClientOptions> options,
            ILogger<AuthorizationCodeTokenClient> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<string> GetAccessTokenAsync(AuthorizationCodeTokenRequest? request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                return string.Empty;
            }

            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _options.IdsHost,
                Policy = new DiscoveryPolicy()
                {
                    RequireHttps = false
                }
            }, cancellationToken);
            if (discovery.IsError)
            {
                _logger.LogError(discovery.Error, discovery.Json.ToString());
                throw new Exception($"Failed to get discovery document: {discovery.Error}");
            }

            request.Address = discovery.TokenEndpoint;

            var response = await _httpClient.RequestAuthorizationCodeTokenAsync(request, cancellationToken);

            if (response.IsError)
            {
                _logger.LogError(response.Error, response.Json.ToString());
                throw new Exception($"Failed to get token: {response.Error}");
            }

            return response.AccessToken;
        }
    }
}
