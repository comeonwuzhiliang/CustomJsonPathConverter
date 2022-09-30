using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonPathConverter.ColumnMapper.NewObject.Test.Trials
{
    public class GenerateNewObject
    {
        [Fact]
        public void GenerateOneLayerObject()
        {
            string json = "{\"name\":\"zhangsan\",\"age\":27}";

            JObject jObjectNew = new JObject();

            JToken jsonToken = JToken.Parse(json);

            var name = jsonToken!.SelectToken("name");

            var age = jsonToken.SelectToken("age");

            Assert.Equal(JTokenType.String, name!.Type);

            Assert.Equal(JTokenType.Integer, age!.Type);

            jObjectNew.Add("Name", name);

            jObjectNew.Add("Age", age);

            var jObjectToDic = jObjectNew.ToObject<IDictionary<string, object>>();

            Assert.Equal(name, jObjectToDic!["Name"]);
            Assert.Equal(age, jObjectToDic!["Age"]);
        }

        [Fact]
        public void GenerateTwoLayerObject()
        {
            string json = "{\"name\":\"zhangsan\",\"address\":{\"province\":\"江苏省\",\"city\":\"盐城市\",\"county\":\"盐都区\"}}";

            JObject jObjectNew = new JObject();

            JToken jsonToken = JToken.Parse(json);

            var name = jsonToken!.SelectToken("name");

            jObjectNew.Add("Name", name);

            var address = jsonToken.SelectToken("address");

            JObject addressJsonObject = new JObject();

            var province = address!.SelectToken("province");

            addressJsonObject.Add("Province", province!);

            var city = address!.SelectToken("city");

            addressJsonObject.Add("City", city!);

            var county = address!.SelectToken("county");

            addressJsonObject.Add("County", county!);

            jObjectNew.Add("Address", JToken.Parse(addressJsonObject.ToString()));

            Assert.Equal(JTokenType.String, name!.Type);

            Assert.Equal(JTokenType.Object, address!.Type);

            var jObjectToDic = jObjectNew.ToObject<IDictionary<string, object>>();

            Assert.Equal(name, jObjectToDic!["Name"]!.ToString());

            var jObjectAddress = jObjectToDic!["Address"];

            Assert.NotNull(jObjectAddress);

            var jObjectAddressAsJObject = jObjectAddress as JObject;

            var jObjectAddressAsJObjectToDic = jObjectAddressAsJObject!.ToObject<IDictionary<string, object>>();

            Assert.Equal(province, jObjectAddressAsJObjectToDic!["Province"]!.ToString());

            Assert.Equal(city, jObjectAddressAsJObjectToDic!["City"]!.ToString());

            Assert.Equal(county, jObjectAddressAsJObjectToDic!["County"]!.ToString());
        }

        [Fact]
        public void GenerateNoLayerArrayObject()
        {
            string json = "[{\"name\":\"zhangsan\",\"age\":27},{\"name\":\"lisi\",\"age\":28}]";

            JToken jsonToken = JToken.Parse(json);

            Assert.Equal(JTokenType.Array, jsonToken.Type);

            JArray jArray = new JArray();

            foreach (var item in jsonToken)
            {
                JObject jObjectNew = new JObject();

                var name = item!.SelectToken("name");

                var age = item.SelectToken("age");

                Assert.Equal(JTokenType.String, name!.Type);

                Assert.Equal(JTokenType.Integer, age!.Type);

                jObjectNew.Add("Name", name);

                jObjectNew.Add("Age", age);

                var jObjectToDic = jObjectNew.ToObject<IDictionary<string, object>>();

                Assert.Equal(name, jObjectToDic!["Name"]);

                Assert.Equal(age, jObjectToDic!["Age"]);

                jArray.Add(jObjectNew);
            }

            Assert.True(1 == 1);
        }
    }
}
