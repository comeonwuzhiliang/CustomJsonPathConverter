using JsonPathConverter.Abstractions;
using JsonPathConverter.Newtonsoft.Helper;

namespace JsonPathConverter.ColumnMapper.NewObject
{
    public class ColumnMapperNewObject : IJsonColumnMapper
    {
        private readonly IEnumerable<JsonPropertyFormatFunction>? _jsonPropertyFormatFunctions;
        public ColumnMapperNewObject(IEnumerable<JsonPropertyFormatFunction>? jsonPropertyFormatFunctions = null)
        {
            _jsonPropertyFormatFunctions = jsonPropertyFormatFunctions;
        }

        public JsonMapperArray MapToCollection(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            return new GenerateNewObject(_jsonPropertyFormatFunctions).MapArray(jsonSourceStr, jsonPathRoot) ?? new JsonMapperArray { };
        }

        public JsonMapperObject MapToObjectByTemplate(string jsonTemplate, string jsonSourceStr)
        {
            JsonPathRoot jsonPathRoot = new JsonPathRoot("$");

            JsonTemplateConvertRelation jsonTemplateConvertRelation = new JsonTemplateConvertRelation();

            var relations = jsonTemplateConvertRelation.ConvertRelations(jsonTemplate);

            if (relations == null || relations.Any() == false)
            {
                return new JsonMapperObject() { };
            }

            foreach (var relation in relations)
            {
                jsonPathRoot.AddJsonPathMapper(relation);
            }

            return new GenerateNewObject(_jsonPropertyFormatFunctions).MapperObject(jsonSourceStr, jsonPathRoot) ?? new JsonMapperObject { };
        }

        public JsonMapperArray MapToCollectionByTemplate(string jsonTemplate, string jsonSourceStr)
        {
            JsonPathRoot jsonPathRoot = new JsonPathRoot("$");

            JsonTemplateConvertRelation jsonTemplateConvertRelation = new JsonTemplateConvertRelation();

            var relations = jsonTemplateConvertRelation.ConvertRelations(jsonTemplate);

            if (relations == null || relations.Any() == false)
            {
                return new JsonMapperArray() { };
            }

            foreach (var relation in relations)
            {
                jsonPathRoot.AddJsonPathMapper(relation);
            }

            return new GenerateNewObject(_jsonPropertyFormatFunctions).MapArray(jsonSourceStr, jsonPathRoot) ?? new JsonMapperArray { };
        }

        public TData? CaptureObject<TData>(string jsonSourceStr, string path)
        {
            return new CaptureObject().Capture<TData>(jsonSourceStr, path);
        }
    }
}