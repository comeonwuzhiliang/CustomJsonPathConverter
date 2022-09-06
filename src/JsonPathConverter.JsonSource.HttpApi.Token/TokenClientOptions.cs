using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonPathConverter.JsonSource.HttpApi.Token
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
