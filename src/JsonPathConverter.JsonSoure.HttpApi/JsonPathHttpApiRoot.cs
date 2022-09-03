using JsonPathConverter.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonPathConverter.JsonSoure.HttpApi
{
    public class JsonHttpApiRequestSource : IJsonRequestSource
    {
        public JsonHttpApiRequestSource(HttpRequestMessage httpRequestMessage)
        {
            HttpRequestMessage = httpRequestMessage;
        }

        public HttpRequestMessage HttpRequestMessage { get; set; }


    }
}
