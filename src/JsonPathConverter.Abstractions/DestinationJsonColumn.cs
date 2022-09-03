namespace JsonPathConverter.Abstractions
{
    public record DestinationJsonColumn
    {
        public string? Name { get; set; }

        public string? Code { get; set; }

        public string? Type { get; set; }
    }
}
