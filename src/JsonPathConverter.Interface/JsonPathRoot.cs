namespace JsonPathConverter.Interface
{
    public abstract class JsonPathRoot
    {
        public string? RootPath { get; }

        public ICollection<JsonPathMapperRelation> JsonPathMapperRelations { get; }

        public IEnumerable<DestinationJsonColumn>? DestinationJsonColumns { get; }

        public JsonPathRoot(string rootPath, IEnumerable<DestinationJsonColumn>? destinationJsonColumns)
        {
            RootPath = rootPath;
            JsonPathMapperRelations = new List<JsonPathMapperRelation>();
            DestinationJsonColumns = destinationJsonColumns;
        }

        // prevent duplicate items when initializing the collection through the method
        public virtual void AddJsonPathMapper(JsonPathMapperRelation jsonPathMapperRelation)
        {
            if (!JsonPathMapperRelations.Contains(jsonPathMapperRelation))
            {
                JsonPathMapperRelations.Add(jsonPathMapperRelation);
            }
        }

        protected virtual bool CheckJsonSource() => true;

    }
}
