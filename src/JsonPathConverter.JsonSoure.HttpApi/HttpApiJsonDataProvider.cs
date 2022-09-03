using JsonPathConverter.Abstractions;
using Microsoft.Extensions.Logging;

namespace JsonPathConverter.JsonSoure.HttpApi
{
    public class HttpApiJsonDataProvider : IJsonDataProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IJsonColumnMapper _jsonColumnMapper;

        public HttpApiJsonDataProvider(
            IHttpClientFactory httpClientFactory,
            ILogger<HttpApiJsonDataProvider> logger,
            IJsonColumnMapper jsonColumnMapper
            )
        {
            _httpClient = httpClientFactory.CreateClient("HttpApiJsonDataProvider_RequestJsonDataProviderUri");
            _logger = logger;
            _jsonColumnMapper = jsonColumnMapper;
        }

        public async Task<IEnumerable<IDictionary<string, object?>>?> GetJsonData(JsonPathRoot jsonPathRoot, CancellationToken cancellationToken)
        {
            if (jsonPathRoot is JsonPathHttpApiRoot)
            {
                JsonPathHttpApiRoot jsonPathHttpApiRoot = (JsonPathHttpApiRoot)jsonPathRoot;

                var result = await _httpClient.SendAsync(jsonPathHttpApiRoot.HttpRequestMessage, cancellationToken);

                result.EnsureSuccessStatusCode();

                var apiJsonResult = await result.Content.ReadAsStringAsync(cancellationToken);

                var jsonObj = _jsonColumnMapper.MapToCollection(apiJsonResult, jsonPathRoot);

                return jsonObj.MapData;
            }
            else
            {
                _logger.LogError($"method {nameof(GetJsonData)} of class HttpApiJsonDataProvider need {nameof(JsonPathHttpApiRoot)} parameter");
                throw new ArgumentException($"need {nameof(JsonPathHttpApiRoot)} parameter");
            }
        }
    }
}
