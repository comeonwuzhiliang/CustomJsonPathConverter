namespace JsonPathConverter.FakeObject
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
