using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace JsonPathConverter.JsonSoure.HttpApi
{
    public class AccessTokenDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccessTokenDelegatingHandler(
            ILogger<AccessTokenDelegatingHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains("Authorization"))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var attachAccessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            request.SetBearerToken(attachAccessToken);

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
