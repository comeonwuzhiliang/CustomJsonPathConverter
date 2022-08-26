using JsonPathConverter.Interface;

namespace JsonPathConverter.HttpApi
{
    public class JsonPathHttpApiRoot : JsonPathRoot
    {
        public string HttpApiName { get; set; } = string.Empty;

        public HttpRequestMessage HttpRequestMessage { get; set; }

        public JsonPathHttpApiRoot(
            string rootPath,
            HttpRequestMessage httpRequestMessage,
            IEnumerable<DestinationJsonColumn>? destinationJsonColumns)
           : base(rootPath, destinationJsonColumns)
        {
            this.HttpRequestMessage = httpRequestMessage;
        }

        protected override bool CheckJsonSource() => true;
    }
}
