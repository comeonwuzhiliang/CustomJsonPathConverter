namespace JsonPathConverter.Abstractions
{
    public record JsonPathMapperRelation
    {
        /// <summary>
        /// 目标Json列Code
        /// </summary>
        public string? DestinationJsonColumnCode { get; set; }

        /// <summary>
        /// 目标类类型
        /// </summary>
        public DestinationPropertyTypeEnum DestinationPropertyType { get; set; } = DestinationPropertyTypeEnum.Property;

        /// <summary>
        /// 子级映射关系
        /// </summary>
        public IEnumerable<JsonPathMapperRelation>? ChildRelations { get; set; }

        /// <summary>
        /// 来源的Json Path
        /// </summary>
        public string? SourceJsonPath { get; set; }
    }
}
