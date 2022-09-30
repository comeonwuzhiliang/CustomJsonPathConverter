using Microsoft.Extensions.Configuration;

namespace JsonPathConverter.JsonSource.HttpApi.Oauth
{
    public class TokenClientOptions
    {
        /// <summary>
        /// ids宿主机
        /// </summary>
        public string? IdsHost { get; set; }

        /// <summary>
        /// oauth2.0 类型
        /// </summary>
        public string? GrantType { get; set; }

        public IConfigurationSection? TokenRequest { get; set; }
    }
}
