namespace JsonPathConverter.Interface
{
    public record JsonPathMapperRelation
    {
        public string? DestinationJsonColumnCode { get; set; }

        public string? SourceJsonPath { get; set; }
    }
}
