using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonPathConverter.ColumnMapper.ReplaceKey.Test.Trials
{
    public class JsonSelectToken
    {
        [Fact]
        public void NoLayerObject()
        {
            string json = "{\"name\":\"zhangsan\",\"age\":27}";

            string root = "$";

            JToken jsonToken = JToken.Parse(json);

            var token = jsonToken.SelectToken(root);

            var name = token!.SelectToken("name")?.ToString();

            var age = token.SelectToken("age")?.ToString();

            Assert.Equal("zhangsan", name);

            Assert.Equal("27", age);
        }

        [Fact]
        public void LayerObject()
        {
            string json = "{\"name\":\"zhangsan\",\"address\":{\"province\":\"江苏省\",\"city\":\"盐城市\",\"county\":\"盐都区\"}}";

            string root = "$.address";

            JToken jsonToken = JToken.Parse(json);

            var token = jsonToken.SelectToken(root);

            var province = token!.SelectToken("province")!.ToString();

            var city = token.SelectToken("city")!.ToString();

            var county = token.SelectToken("county")!.ToString();

            Assert.Equal("江苏省", province);

            Assert.Equal("盐城市", city);

            Assert.Equal("盐都区", county);
        }

        [Fact]
        public void SecondLayerObject()
        {
            string json = "{\"name\":\"zhangsan\",\"address\":{\"detail\":{\"province\":\"江苏省\",\"city\":\"盐城市\",\"county\":\"盐都区\"}}}";

            string root = "$.address.detail";

            JToken jsonToken = JToken.Parse(json);

            var token = jsonToken.SelectToken(root);

            var province = token!.SelectToken("province")!.ToString();

            var city = token.SelectToken("city")!.ToString();

            var county = token.SelectToken("county")!.ToString();

            Assert.Equal("江苏省", province);

            Assert.Equal("盐城市", city);

            Assert.Equal("盐都区", county);
        }


        [Fact]
        public void NoLayerArray()
        {
            string json = "[{\"name\":\"zhangsan\",\"age\":27},{\"name\":\"lisi\",\"age\":28}]";

            string root = "$";

            JToken jsonToken = JToken.Parse(json);

            var token = jsonToken.SelectToken(root);

            Assert.True(token!.Count() == 2);
        }

        [Fact]
        public void LayerArray()
        {
            string json = "{\"name\":\"zhangsan\",\"age\":27,\"scores\":[{\"score\":98,\"subject\":\"math\"},{\"score\":5,\"subject\":\"language\"}]}";

            string root = "$.scores";

            JToken jsonToken = JToken.Parse(json);

            var token = jsonToken.SelectToken(root);

            Assert.True(token!.Count() == 2);
        }

        [Fact]
        public void SecondLayerArray()
        {
            string json = "{\"name\":\"zhangsan\",\"age\":27,\"other\":{\"scores\":[{\"score\":98,\"subject\":\"math\"},{\"score\":5,\"subject\":\"language\"}]}}";

            string root = "$.other.scores";

            JToken jsonToken = JToken.Parse(json);

            var token = jsonToken.SelectToken(root);

            Assert.True(token!.Count() == 2);
        }
    }
}
