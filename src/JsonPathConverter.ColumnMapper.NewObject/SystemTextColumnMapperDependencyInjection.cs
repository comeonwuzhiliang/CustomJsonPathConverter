using JsonPathConverter.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace JsonPathConverter.ColumnMapper.SystemText
{
    public static class ColumnMapperNewObjectDependencyInjection
    {
        public static IServiceCollection AddColumnMapperNewObject(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IJsonColumnMapper, ColumnMapperNewObject>();
            return serviceCollection;
        }
    }
}
