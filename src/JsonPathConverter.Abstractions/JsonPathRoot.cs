namespace JsonPathConverter.Abstractions
{
    public class JsonPathRoot
    {
        public string? RootPath { get; }

        public ICollection<JsonPathMapperRelation> JsonPathMapperRelations { get; }

        public JsonPathRoot(string rootPath)
        {
            RootPath = rootPath;
            JsonPathMapperRelations = new List<JsonPathMapperRelation>();
        }

        // prevent duplicate items when initializing the collection through the method
        public virtual void AddJsonPathMapper(JsonPathMapperRelation jsonPathMapperRelation)
        {
            if (!JsonPathMapperRelations.Contains(jsonPathMapperRelation))
            {
                JsonPathMapperRelations.Add(jsonPathMapperRelation);
            }
        }
    }
}
