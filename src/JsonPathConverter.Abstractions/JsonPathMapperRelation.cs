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

        ///// <summary>
        ///// 父节点
        ///// </summary>
        //public JsonPathMapperRelation? Parent { get; private set; }

        ///// <summary>
        ///// 设置父节点
        ///// TODO:使用<seealso cref="INotifyPropertyChanged">和<seealso cref="INotifyCollectionChanged"/>
        ///// </summary>
        //internal void SetParent()
        //{
        //    if (ChildRelations != null)

        //        foreach (var item in ChildRelations)
        //        {
        //            item.Parent = this;
        //            if (item.ChildRelations != null)
        //                foreach (var item2 in item.ChildRelations)
        //                {
        //                    item2.SetParent();
        //                }
        //        }
        //}
    }
}
