namespace JsonPathConverter.Abstractions
{
    public class JsonPropertyFormatFunction
    {
        public string? FormatKey { get; set; }

        public Func<object>? FormatFunction { get; set; }
    }
}
