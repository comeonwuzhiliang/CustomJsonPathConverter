using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonPathConverter.HttpApi.Test.Fake
{
    public class UserAction
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public UserOtherInformation OtherInformation { get; set; } = new UserOtherInformation();

        public List<UserLog> UserLogs { get; set; } = new List<UserLog>();

        public List<UserLog>? UserLogs2 { get; set; }
    }
}
