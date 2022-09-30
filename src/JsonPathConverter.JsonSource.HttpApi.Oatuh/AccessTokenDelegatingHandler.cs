using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using System.Net;

namespace JsonPathConverter.JsonSource.HttpApi.Oauth
{
    public class AccessTokenDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;

        public AccessTokenDelegatingHandler(
            ILogger<AccessTokenDelegatingHandler> logger,
            ITokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains("Authorization"))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var accessToken = await _tokenService.GetToken();

            request.SetBearerToken(accessToken);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogError("request is unauthorized,need retry: {0}", response.Headers.WwwAuthenticate.ToString());
                throw new HttpRequestException("request is unauthorized,need retry.");
            }

            return response;
        }
    }
}

