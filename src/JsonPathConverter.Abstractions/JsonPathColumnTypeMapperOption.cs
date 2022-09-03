namespace JsonPathConverter.Abstractions
{
    public class JsonPathColumnTypeMapperOption
    {
        public JsonPathColumnTypeMapperOption()
        {
            JsonColumnTypeMappers = new List<IJsonColumnTypeMapper>();
        }
        public ICollection<IJsonColumnTypeMapper> JsonColumnTypeMappers { get; }
    }
}
