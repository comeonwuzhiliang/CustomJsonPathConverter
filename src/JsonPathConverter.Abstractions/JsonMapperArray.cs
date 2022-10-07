namespace JsonPathConverter.Abstractions
{
    public class JsonMapperArray
    {
        public IEnumerable<IDictionary<string, object?>>? Data { get; set; }

        public string? Json { get; set; }
    }
}
