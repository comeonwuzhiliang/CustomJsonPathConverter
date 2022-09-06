using JsonPathConverter.Abstractions;
using JsonPathConverter.JsonSource.HttpApi.Abstractions;
using Microsoft.Extensions.Logging;

namespace JsonPathConverter.JsonSoure.HttpApi
{
    public class HttpApiJsonDataProvider : IJsonDataProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public HttpApiJsonDataProvider(
            IHttpClientFactory httpClientFactory,
            ILogger<HttpApiJsonDataProvider> logger
            )
        {
            _httpClient = httpClientFactory.CreateClient("HttpApiJsonDataProvider_RequestJsonDataProviderUri");
            _logger = logger;
        }

        public async Task<string> GetJsonDataAsync(IJsonRequestSource requestSource, CancellationToken cancellationToken = default)
        {
            if (requestSource is JsonHttpApiRequestSource)
            {
                JsonHttpApiRequestSource httpApiRequestSource = (JsonHttpApiRequestSource)requestSource;

                var result = await _httpClient.SendAsync(httpApiRequestSource.HttpRequestMessage, cancellationToken);

                result.EnsureSuccessStatusCode();

                var apiJsonResult = await result.Content.ReadAsStringAsync(cancellationToken);

                return apiJsonResult;
            }
            else
            {
                _logger.LogError($"method {nameof(GetJsonDataAsync)} of class HttpApiJsonDataProvider need {nameof(JsonHttpApiRequestSource)} parameter");
                throw new ArgumentException($"need {nameof(JsonHttpApiRequestSource)} parameter");
            }
        }

        public async Task GetNoJsonDataAsync(IJsonRequestSource requestSource, CancellationToken cancellationToken = default)
        {
            if (requestSource is JsonHttpApiRequestSource)
            {
                JsonHttpApiRequestSource httpApiRequestSource = (JsonHttpApiRequestSource)requestSource;

                var result = await _httpClient.SendAsync(httpApiRequestSource.HttpRequestMessage, cancellationToken);

                result.EnsureSuccessStatusCode();
            }
            else
            {
                _logger.LogError($"method {nameof(GetNoJsonDataAsync)} of class HttpApiJsonDataProvider need {nameof(JsonHttpApiRequestSource)} parameter");
                throw new ArgumentException($"need {nameof(JsonHttpApiRequestSource)} parameter");
            }
        }
    }
}
