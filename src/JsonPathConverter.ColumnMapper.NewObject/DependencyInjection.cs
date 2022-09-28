using JsonPathConverter.Abstractions;
using JsonPathConverter.ColumnMapper.NewObject;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddColumnMapperNewObject(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IJsonColumnMapper, ColumnMapperNewObject>();
            return serviceCollection;
        }
    }
}
