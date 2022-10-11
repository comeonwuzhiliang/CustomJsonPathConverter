using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonPathConverter.ColumnMapper.JsonGenerateObject.Test.Trials
{
    public class JsonObjectTypeAnalysis
    {
        [Fact]
        public void JsonObject()
        {
            string json =
                "{\"name\":\"\",\"createDataRange\":{\"start\":\"\",\"end\":\"\"},\"hyperLinkContext\":\"\",\"hyperLinkUrl\":\"\",\"departments\":[{\"id\":\"\",\"name\":\"\",\"role\":{\"id\":\"\",\"name\":\"\"},\"leaders\":[\"\"]},{\"id\":\"\",\"name\":\"\",\"role\":{\"id\":\"\",\"name\":\"\"},\"leaders\":[\"\"]}],\"phone1\":\"\",\"phone2\":\"\",\"suggests\":[\"\"],\"suggest1\":\"\",\"suggest2\":\"\",\"suggest3\":\"\"}";

            var jToken = JToken.Parse(json);

            foreach (var item in jToken)
            {
                if (item.Path == "name")
                {
                    Assert.True(item.Type == JTokenType.Property);

                    var name = jToken.SelectToken("name");

                    Assert.True(name is { Type: JTokenType.String });
                }
                else if (item.Path == "createDataRange")
                {
                    Assert.True(item.Type == JTokenType.Property);

                    var createDataRange = jToken.SelectToken("createDataRange");

                    Assert.True(createDataRange is { Type: JTokenType.Object });

                    if (createDataRange != null)
                        foreach (var createDataRangeItem in createDataRange)
                        {
                            Assert.True(createDataRangeItem.Type == JTokenType.Property);
                            var path = createDataRangeItem.Path;

                            var createDataRangeItemToken = jToken.SelectToken(path);

                            Assert.True(createDataRangeItemToken is { Type: JTokenType.String });
                        }
                }
                else if (item.Path == "hyperLinkContext")
                {
                    Assert.True(item.Type == JTokenType.Property);

                    var hyperLinkContext = jToken.SelectToken("hyperLinkContext");

                    Assert.True(hyperLinkContext is { Type: JTokenType.String });
                }
                else if (item.Path == "hyperLinkUrl")
                {
                    Assert.True(item.Type == JTokenType.Property);

                    var hyperLinkUrl = jToken.SelectToken("hyperLinkUrl");

                    Assert.True(hyperLinkUrl is { Type: JTokenType.String });
                }
                else if (item.Path == "departments")
                {
                    Assert.True(item.Type == JTokenType.Property);

                    var departments = jToken.SelectToken("departments");

                    Assert.True(departments is { Type: JTokenType.Array });

                    if (departments != null)
                        foreach (var department in departments)
                        {
                            Assert.True(department.Type == JTokenType.Object);

                            var departmentId = department.SelectToken("id");

                            Assert.True(departmentId is { Type: JTokenType.String });

                            var departmentName = department.SelectToken("name");

                            Assert.True(departmentName is { Type: JTokenType.String });

                            var role = department.SelectToken("role");

                            Assert.True(role is { Type: JTokenType.Object });

                            if (role != null)
                            {
                                var roleId = role.SelectToken("id");

                                Assert.True(roleId is { Type: JTokenType.String });

                                var roleName = role.SelectToken("name");

                                Assert.True(roleName is { Type: JTokenType.String });
                            }

                            var leaders = department.SelectToken("leaders");

                            Assert.True(leaders is { Type: JTokenType.Array });

                            if (leaders != null)
                                foreach (var leader in leaders)
                                {
                                    Assert.True(leader is { Type: JTokenType.String });
                                }
                        }
                }
                else if (item.Path == "phone1")
                {
                    Assert.True(item.Type == JTokenType.Property);

                    var phone1 = jToken.SelectToken("phone1");

                    Assert.True(phone1 is { Type: JTokenType.String });
                }
                else if (item.Path == "phone2")
                {
                    Assert.True(item.Type == JTokenType.Property);

                    var phone2 = jToken.SelectToken("phone2");

                    Assert.True(phone2 is { Type: JTokenType.String });
                }
                else if (item.Path == "suggests")
                {
                    Assert.True(item.Type == JTokenType.Property);

                    var suggests = jToken.SelectToken("suggests");

                    Assert.True(suggests is { Type: JTokenType.Array });
                    
                    if (suggests != null)
                        foreach (var suggest in suggests)
                        {
                            Assert.True(suggest is { Type: JTokenType.String });
                        }
                }
                else if (item.Path == "suggest1")
                {
                    Assert.True(item.Type == JTokenType.Property);

                    var suggest1 = jToken.SelectToken("suggest1");

                    Assert.True(suggest1 is { Type: JTokenType.String });
                }
                else if (item.Path == "suggest2")
                {
                    Assert.True(item.Type == JTokenType.Property);

                    var suggest2 = jToken.SelectToken("suggest2");

                    Assert.True(suggest2 is { Type: JTokenType.String });
                }
                else if (item.Path == "suggest3")
                {
                    Assert.True(item.Type == JTokenType.Property);

                    var suggest3 = jToken.SelectToken("suggest3");

                    Assert.True(suggest3 is { Type: JTokenType.String });
                }
            }
        }
    }
}