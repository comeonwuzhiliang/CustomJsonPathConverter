using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonPathConverter.Interface
{
    public interface IJsonColumnMapper
    {
        // string MapToStr(string jsonSourceStr, IEnumerable<DestinationJsonColumn>? destinationJsonColumns, IEnumerable<JsonPathMapperRelation>? jsonPathMapperRelations);

        List<Dictionary<string, object?>> MapToCollection(string jsonSourceStr, IEnumerable<DestinationJsonColumn>? destinationJsonColumns, IEnumerable<JsonPathMapperRelation>? jsonPathMapperRelations);

        Dictionary<string, object?> MapToDic(string jsonSourceStr, IEnumerable<DestinationJsonColumn>? destinationJsonColumns, IEnumerable<JsonPathMapperRelation>? jsonPathMapperRelations);
    }
}
