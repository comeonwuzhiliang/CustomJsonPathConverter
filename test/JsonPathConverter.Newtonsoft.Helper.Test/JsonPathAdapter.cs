using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonPathConverter.Newtonsoft.Helper.Test
{
    public class JsonPathAdapter
    {
        [Fact]
        public void NoLayerObject()
        {
            string json = "{\"name\":\"zhangsan\",\"age\":27,\"scores\":[{\"score\":98,\"subject\":\"math\",\"details\":[{\"id\":1,\"score\":10},{\"id\":2,\"score\":10}]},{\"score\":5,\"subject\":\"language\"}]}";

            Helper.JsonPathAdapter jsonPathAdapter = new Helper.JsonPathAdapter();

            JToken jToken = JObject.Parse(json);

            string scorePathResult = jsonPathAdapter.Adapter("$.scores.score", jToken);

            Assert.Equal("$.scores[*].score", scorePathResult);

            string scorePathResult2 = jsonPathAdapter.Adapter("$.scores[*].subject", jToken);

            Assert.Equal("$.scores[*].subject", scorePathResult2);

            string scorePathResult3 = jsonPathAdapter.Adapter("$.scores[0].subject", jToken);

            Assert.Equal("$.scores[0].subject", scorePathResult3);

            string scorePathResult4 = jsonPathAdapter.Adapter("$.scores[1].subject", jToken);

            Assert.Equal("$.scores[1].subject", scorePathResult4);

        }
    }
}
