using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonPathConverter.Interface
{
    public class JsonPathColumnMapperOption
    {
        public JsonPathColumnMapperOption()
        {
            JsonColumnMappers = new List<IJsonColumnMapper>();
        }
        public ICollection<IJsonColumnMapper> JsonColumnMappers { get; }
    }
}
