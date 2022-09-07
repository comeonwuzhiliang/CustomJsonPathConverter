using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace JsonPathConverter.JsonSource.HttpApi.Token
{
    public class AttachTokenClient : ITokenClient<AttachTokenRequest>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AttachTokenClient(
            ILogger<AttachTokenClient> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetAccessTokenAsync(AttachTokenRequest? request, CancellationToken cancellationToken = default)
        {
            var attachAccessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            return await Task.FromResult(attachAccessToken);
        }
    }

    public class AttachTokenRequest : TokenRequest
    {

    }
}
