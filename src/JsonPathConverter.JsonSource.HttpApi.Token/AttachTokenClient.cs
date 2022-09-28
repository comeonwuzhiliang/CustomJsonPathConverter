using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace JsonPathConverter.JsonSource.HttpApi.Oauth
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
            _logger.LogInformation($"token:{attachAccessToken}");
            try
            {
                var token = attachAccessToken[0].Replace("Bearer ", "");

                return await Task.FromResult(token);
            }
            catch
            {
                return "";
            }
        }
    }

    public class AttachTokenRequest : TokenRequest
    {

    }
}
