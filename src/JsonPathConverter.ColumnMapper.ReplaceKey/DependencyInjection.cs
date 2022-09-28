using JsonPathConverter.Abstractions;
using JsonPathConverter.ColumnMapper.ReplaceKey;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddColumnMapperReplaceKey(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IJsonColumnMapper, ColumnMapperReplaceKey>();
            return serviceCollection;
        }
    }
}
