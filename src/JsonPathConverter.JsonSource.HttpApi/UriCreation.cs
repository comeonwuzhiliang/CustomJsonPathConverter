using JsonPathConverter.JsonSource.HttpApi.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace JsonPathConverter.JsonSource.HttpApi
{
    public class UriCreation : IUriCreation
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UriCreation(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Uri CreateUri(string uriString)
        {
            ArgumentNullException.ThrowIfNull(uriString, nameof(uriString));

            Uri uri;

            try
            {
                uri = new Uri(uriString);
            }
            catch (UriFormatException)
            {
                var request = _httpContextAccessor.HttpContext.Request;

                StringValues? referers = request.Headers["referer"];

                var referer = referers?.FirstOrDefault();

                string requestScheme = string.Empty;
                string requestHost = string.Empty;

                if (referer != null)
                {
                    var refererUri = new Uri(referer);

                    requestScheme = refererUri.Scheme;

                    requestHost = refererUri.Authority;
                }
                else
                {
                    requestScheme = request.Scheme;

                    requestHost = request.Host.ToString();
                }

                uri = new Uri($"{requestScheme}://{requestHost}/{uriString.TrimStart('/')}");
            }

            return uri;
        }
    }
}
