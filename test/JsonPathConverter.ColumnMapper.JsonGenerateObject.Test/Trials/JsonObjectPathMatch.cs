using Xunit;
using Newtonsoft.Json.Linq;
using JsonPathConverter.Newtonsoft.Helper;

namespace JsonPathConverter.ColumnMapper.JsonGenerateObject.Test.Trials
{
    public class JsonObjectPathMatch
    {
        [Fact]
        public void JsonObjectTemplate()
        {
            string jsonTemplate =
                "{\"name\":\"$.name\",\"createDataRange\":{\"start\":\"$.startCreateData\",\"end\":\"$.endCreateData\"},\"hyperLinkContext\":\"$.hyperLink.context\",\"hyperLinkUrl\":\"$.hyperLink.url\",\"departments\":[{\"id\":\"$.department.id\",\"name\":\"$.department.name\",\"leaders\":[\"$.leaders.name\"]}],\"role\":{\"id\":\"$.roles[0].id\",\"name\":\"$.roles[0].name\"},\"phone1\":\"$.phone[0]\",\"phone2\":\"$.phone[1]\",\"suggests\":[\"$.define.suggest.message\"],\"suggest1\":\"$.define.suggest.message[0]\",\"suggest2\":\"$.define.suggest.message[1]\",\"suggest3\":\"$.define.suggest.message[2]\"}";

            string jsonSource =
                "{\"name\":\"azir\",\"startCreateData\":\"2022-10-13 00:00:00\",\"endCreateData\":\"2022-10-14 00:00:00\",\"hyperLink\":{\"context\":\"百度一下\",\"url\":\"https://www.baidu.com\"},\"department\":{\"id\":\"111\",\"name\":\"研发部\"},\"leaders\":[{\"name\":\"azirliang\"},{\"name\":\"azir\"}],\"roles\":[{\"id\":\"2222\",\"name\":\"role2222\"}],\"phone\":[\"10086\",\"10010\",\"10001\"],\"define\":{\"suggest\":{\"message\":[\"aaa\",\"bbb\"]}}}";


            JObject jObject = new JObject();

            var jToken = JToken.Parse(jsonTemplate);

            var jsonSourceToken = JToken.Parse(jsonSource);

            foreach (var item in jToken)
            {
                if (item.Path == "name")
                {
                    var name = jToken.SelectToken("name");

                    var jsonSourcePath = name!.Value<string>();

                    var jsonSourceName = jsonSourceToken.SelectToken(jsonSourcePath!);

                    jObject.Add("name", jsonSourceName!.ToObject<string>());
                }
                else if (item.Path == "createDataRange")
                {
                    var createDataRange = jToken.SelectToken("createDataRange");

                    JObject jCreateDataRange = new JObject();

                    if (createDataRange != null)
                        foreach (var createDataRangeItem in createDataRange)
                        {
                            var path = createDataRangeItem.Path;

                            var jsonSourcePath = jToken.SelectToken(path)!.Value<string>();

                            var createDataRangeItemToken = jsonSourceToken.SelectToken(jsonSourcePath!);


                            var propertyName = path.Split('.').Last();

                            jCreateDataRange.Add(propertyName, createDataRangeItemToken!.ToObject<string>());
                        }

                    jObject.Add("createDataRange", jCreateDataRange);
                }
                else if (item.Path == "hyperLinkContext")
                {
                    var hyperLinkContext = jToken.SelectToken("hyperLinkContext");

                    var hyperLinkContextSourcePath = hyperLinkContext!.Value<string>();

                    var sourceHyperLinkContext =
                        jsonSourceToken.SelectToken(hyperLinkContextSourcePath!)!.Value<string>();

                    jObject.Add("hyperLinkContext", sourceHyperLinkContext);
                }
                else if (item.Path == "hyperLinkUrl")
                {
                    var hyperLinkUrl = jToken.SelectToken("hyperLinkUrl");

                    var hyperLinkUrlSourcePath = hyperLinkUrl!.Value<string>();

                    var sourceHyperLinkUrl =
                        jsonSourceToken.SelectToken(hyperLinkUrlSourcePath!)!.Value<string>();

                    jObject.Add("hyperLinkUrl", sourceHyperLinkUrl);
                }
                else if (item.Path == "departments")
                {
                    var departments = jToken.SelectToken("departments");
                    JArray departmentsArray = new JArray();

                    JObject departmentsObject = new JObject();
                    foreach (var departmentItem in departments![0]!)
                    {
                        var path = departmentItem.Path.Split(".").Last();
                        if (path == "id")
                        {
                            var departmentItemIdPath = jToken.SelectToken(departmentItem.Path)!.Value<string>();

                            var id = jsonSourceToken.SelectToken(departmentItemIdPath!)!.Value<string>();

                            departmentsObject.Add("id", id);
                        }
                        else if (path == "name")
                        {
                            var departmentItemNamePath = jToken.SelectToken(departmentItem.Path)!.Value<string>();

                            var name = jsonSourceToken.SelectToken(departmentItemNamePath!)!.Value<string>();

                            departmentsObject.Add("name", name);
                        }
                        else if (path == "leaders")
                        {
                            var departmentItemLeaders = jToken.SelectToken(departmentItem.Path);
                            var departmentItemLeadersPath = departmentItemLeaders![0]!.Value<string>();

                            JsonPathAdapter jsonPathAdapter = new JsonPathAdapter();

                            var sourcePath = jsonPathAdapter.Adapter(departmentItemLeadersPath!, jsonSourceToken);

                            var tokens = jsonSourceToken.SelectTokens(sourcePath);

                            JArray tokenArray = new JArray();

                            foreach (var token in tokens)
                            {
                                tokenArray.Add(token.Value<string>());
                            }

                            departmentsObject.Add("leaders", tokenArray);
                        }

                        departmentsArray.Add(departmentsObject);
                    }

                    jObject.Add("departments", departmentsArray);
                }
                else if (item.Path == "role")
                {
                    var role = jToken.SelectToken("role");

                    JObject jRole = new JObject();

                    if (role != null)
                        foreach (var roleItem in role)
                        {
                            var path = roleItem.Path;

                            var jsonSourcePath = jToken.SelectToken(path)!.Value<string>();

                            var roleItemToken = jsonSourceToken.SelectToken(jsonSourcePath!);

                            var propertyName = path.Split('.').Last();

                            jRole.Add(propertyName, roleItemToken!.ToObject<string>());
                        }

                    jObject.Add("role", jRole);
                }
                else if (item.Path == "phone1")
                {
                    var phone1 = jToken.SelectToken("phone1");
                    var phone1SourcePath = phone1!.Value<string>();

                    var phone1Value = jsonSourceToken.SelectToken(phone1SourcePath!)!.Value<string>();

                    jObject.Add("phone1", phone1Value);
                }
                else if (item.Path == "phone2")
                {
                    var phone2 = jToken.SelectToken("phone2");
                    var phone2SourcePath = phone2!.Value<string>();

                    var phone2Value = jsonSourceToken.SelectToken(phone2SourcePath!)!.Value<string>();

                    jObject.Add("phone2", phone2Value);
                }
                else if (item.Path == "suggests")
                {
                    var suggests = jToken.SelectToken("suggests");

                    var suggestsSourcePath = suggests![0]!.Value<string>();

                    var jsonSourceValue = jsonSourceToken.SelectToken(suggestsSourcePath!);

                    JArray jArray = new JArray();

                    foreach (var jsonSourceValueItem in jsonSourceValue!)
                    {
                        jArray.Add(jsonSourceValueItem);
                    }

                    jObject.Add("suggests", jArray);
                }
                else if (item.Path == "suggest1")
                {
                    var suggest1 = jToken.SelectToken("suggest1");
                    var suggest1SourcePath = suggest1!.Value<string>();
                    var suggest1Value = jsonSourceToken.SelectToken(suggest1SourcePath!)!.Value<string>();

                    jObject.Add("suggest1", suggest1Value);
                }
                else if (item.Path == "suggest2")
                {
                    var suggest2 = jToken.SelectToken("suggest2");
                    var suggest2SourcePath = suggest2!.Value<string>();
                    var suggest2Value = jsonSourceToken.SelectToken(suggest2SourcePath!)!.Value<string>();

                    jObject.Add("suggest2", suggest2Value);
                }
                else if (item.Path == "suggest3")
                {
                    jObject.Add("suggest3", null);
                }
            }

            Assert.True(jObject != null);
        }
    }
}