using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonPathConverter.ColumnMapper.ReplaceKey.Test
{
    public class JsonCaptureObject
    {
        [Fact]
        public void CaptureTotalCount()
        {
            string json = "{\"data\":[{\"name\":\"zhangsan\",\"age\":27},{\"name\":\"lisi\",\"age\":28}],\"totalCount\":500}";

            int totalCount = new ColumnMapperReplaceKey().CaptureObject<int>(json, "$.totalCount");

            Assert.True(totalCount == 500);
        }
    }
}
