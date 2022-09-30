using Xunit;

namespace JsonPathConverter.Newtonsoft.Helper.Test
{
    public class JsonCaptureObject
    {
        [Fact]
        public void CaptureTotalCount()
        {
            string json = "{\"data\":[{\"name\":\"zhangsan\",\"age\":27},{\"name\":\"lisi\",\"age\":28}],\"totalCount\":500}";

            int totalCount = new CaptureObject().Capture<int>(json, "$.totalCount");

            Assert.True(totalCount == 500);
        }

        [Fact]
        public void CaptureArrayObject()
        {
            string json = "{\"data\":[{\"name\":\"zhangsan\",\"age\":27},{\"name\":\"lisi\",\"age\":28}],\"totalCount\":500}";

            string? name = new CaptureObject().Capture<string>(json, "$.data.name");

            Assert.True(name == "zhangsan");

            int age = new CaptureObject().Capture<int>(json, "$.data.age");

            Assert.True(age == 27);

            string[]? names = new CaptureObject().Capture<string[]?>(json, "$.data.name");

            Assert.True(names![0] == "zhangsan");

            Assert.True(names[1] == "lisi");
        }

        [Fact]
        public void CaptureGuid()
        {
            string json = "{\"data\":[{\"name\":\"zhangsan\",\"age\":27,\"id\":\"5c262817-efa9-4697-91b2-2fdf78e9209f\"},{\"name\":\"lisi\",\"age\":28}],\"totalCount\":500}";

            Guid id = new CaptureObject().Capture<Guid>(json, "$.data.id");

            Assert.True(id == Guid.Parse("5c262817-efa9-4697-91b2-2fdf78e9209f"));
        }
    }
}
