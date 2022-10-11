namespace JsonPathConverter.Abstractions;

public class JsonMapperObject
{
    public IDictionary<string, object?>? Data { get; set; }

    public string? Json { get; set; }
}