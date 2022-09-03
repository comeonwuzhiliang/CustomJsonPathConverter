using JsonPathConverter.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace JsonPathConverter.ColumnMapper.ReplaceKey
{
    public static class ColumnMapperReplaceKeyDependencyInjection
    {
        public static IServiceCollection AddColumnMapperReplaceKey(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IJsonColumnMapper, ColumnMapperReplaceKey>();
            return serviceCollection;
        }
    }
}
