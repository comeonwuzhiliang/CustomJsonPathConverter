using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonPathConverter.Newtonsoft.Helper
{
    public class CaptureObject
    {
        public TData? Capture<TData>(string jsonSourceStr, string path)
        {
            var jToken = JToken.Parse(jsonSourceStr);

            IEnumerable<JToken> jTokens;

            if (jToken == null)
            {
                return default;
            }

            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    string jsonPath = path;
                    string jsonPathAdapterResult = new JsonPathAdapter().Adapter(jsonPath, jToken);
                    jTokens = jToken.SelectTokens(jsonPathAdapterResult);
                }
                catch
                {
                    throw new Exception("json来源的数组项配置不正确");
                }
            }
            else
            {
                throw new ArgumentException("路径不能为空");
            }

            if (jTokens?.Any() == false)
            {
                return default;
            }

            if (typeof(TData).IsArray || typeof(TData) == typeof(object))
            {
                var jTokensStr = JsonConvert.SerializeObject(jTokens);
                if (string.IsNullOrEmpty(jTokensStr))
                {
                    return default;
                }
                return JsonConvert.DeserializeObject<TData>(jTokensStr);
            }
            else
            {
                var jTokensStr = jTokens!.First().ToString();

                if (typeof(TData) == typeof(Guid))
                {
                    var guid = Guid.Parse(jTokensStr);
                    return (TData)Convert.ChangeType(guid, typeof(TData));
                }

                if (typeof(TData).IsAssignableTo(typeof(ValueType)) || typeof(TData) == typeof(string))
                {
                    return (TData)Convert.ChangeType(jTokensStr, typeof(TData));
                }

                return JsonConvert.DeserializeObject<TData>(jTokensStr);
            }

        }
    }
}
