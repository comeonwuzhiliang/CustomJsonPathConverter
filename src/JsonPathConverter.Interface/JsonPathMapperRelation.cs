namespace JsonPathConverter.Interface
{
    public record JsonPathMapperRelation
    {
        public string? RootPath { get; set; }

        public string? DestinationJsonColumnCode { get; set; }

        public string? SourceJsonPath { get; set; }
    }
}
