using JsonPathConverter.Abstractions;
using JsonPathConverter.Newtonsoft.Helper;

namespace JsonPathConverter.ColumnMapper.NewObject
{
    public class ColumnMapperNewObject : IJsonColumnMapper
    {
        public JsonMapperArray MapToCollection(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            return new GenerateNewObject().MapArray(jsonSourceStr, jsonPathRoot) ?? new JsonMapperArray { };
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

            return new GenerateNewObject().MapperObject(jsonSourceStr, jsonPathRoot) ?? new JsonMapperObject { };
        }

        public TData? CaptureObject<TData>(string jsonSourceStr, string path)
        {
            return new CaptureObject().Capture<TData>(jsonSourceStr, path);
        }
    }
}