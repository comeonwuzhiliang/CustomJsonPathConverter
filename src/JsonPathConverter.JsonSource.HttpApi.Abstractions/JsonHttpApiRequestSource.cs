using JsonPathConverter.Abstractions;

namespace JsonPathConverter.JsonSource.HttpApi.Abstractions
{
    public class JsonHttpApiRequestSource: IJsonRequestSource
    {
        public JsonHttpApiRequestSource(HttpRequestMessage httpRequestMessage)
        {
            HttpRequestMessage = httpRequestMessage;
        }

        public HttpRequestMessage HttpRequestMessage { get; set; }
    }
}