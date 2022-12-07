namespace JsonPathConverter.Abstractions
{
    public record JsonPathMapperRelation<T> : JsonPathMapperRelation
    {
        public T? OtherInfo { get; set; }
    }
}