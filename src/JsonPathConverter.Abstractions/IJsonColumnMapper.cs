using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonPathConverter.Abstractions
{
    public interface IJsonColumnMapper
    {
        JsonMapResult<IEnumerable<IDictionary<string, object?>>> MapToCollection(string jsonSourceStr, JsonPathRoot jsonPathRoot);

        JsonMapResult<IDictionary<string, object?>> MapToDic(string jsonSourceStr, JsonPathRoot jsonPathRoot);
    }
}
