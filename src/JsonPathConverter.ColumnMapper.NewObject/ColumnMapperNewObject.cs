using JsonPathConverter.Abstractions;
using System.Text.Json;
using static System.Text.Json.JsonElement;

namespace JsonPathConverter.ColumnMapper.NewObject
{
    public class ColumnMapperNewObject : IJsonColumnMapper
    {
        public JsonMapResult<TData> Map<TData>(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            if (typeof(TData).IsArray)
            {
                var collectionResult = MapToCollection(jsonSourceStr, jsonPathRoot);

                var jsonStr = collectionResult.MapJsonStr;

                var jsonData = JsonSerializer.Deserialize<TData>(jsonStr);

                return new JsonMapResult<TData>(jsonData) { MapJsonStr = jsonStr };
            }
            else
            {
                var dicResult = MapToDic(jsonSourceStr, jsonPathRoot);

                var jsonStr = dicResult.MapJsonStr;

                var jsonData = JsonSerializer.Deserialize<TData>(jsonStr);

                return new JsonMapResult<TData>(jsonData) { MapJsonStr = jsonStr };
            }
        }

        public JsonMapResult<IEnumerable<IDictionary<string, object?>>> MapToCollection(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            var list = GenerateObject.MapToList(jsonSourceStr, jsonPathRoot);

            var result = new JsonMapResult<IEnumerable<IDictionary<string, object?>>>(list);

            result.MapJsonStr = JsonSerializer.Serialize(list);

            return result;

        }

        public JsonMapResult<IDictionary<string, object?>> MapToDic(string jsonSourceStr, JsonPathRoot jsonPathRoot)
        {
            var obj = GenerateObject.MapToObject(jsonSourceStr, jsonPathRoot);

            var result = new JsonMapResult<IDictionary<string, object?>>(obj);

            result.MapJsonStr = JsonSerializer.Serialize(obj);

            return result;
        }


    }
}
