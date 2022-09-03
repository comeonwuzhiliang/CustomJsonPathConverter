namespace JsonPathConverter.Abstractions
{
    public record JsonPathMapperRelation
    {
        public string? RootPath { get; set; }

        public string? DestinationJsonColumnCode { get; set; }

        public string? SourceJsonPath { get; set; }

        public string[] GetFileds()
        {
            var fileds = new string[0];
            var filedStr = this.SourceJsonPath;
            if (string.IsNullOrEmpty(filedStr))
            {
                return fileds;
            }
            if (filedStr.StartsWith("$."))
            {
                filedStr = filedStr.Substring(2);

            }
            fileds = filedStr.Split('.');
            return fileds;
        }

        public bool IsValidate()
        {
            if (string.IsNullOrEmpty(SourceJsonPath))
            {
                return false;
            }

            if (string.IsNullOrEmpty(DestinationJsonColumnCode))
            {
                return false;
            }

            return true;
        }
    }
}
