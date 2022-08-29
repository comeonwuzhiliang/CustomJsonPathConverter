using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonPathConverter.HttpApi.Test.Fake
{
    public class UserLog
    {
        public string? Message { get; set; }

        public string? ActionName { get; set; }

        public DateTime Date { get; set; }
    }
}
