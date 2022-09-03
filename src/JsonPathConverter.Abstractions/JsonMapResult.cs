namespace JsonPathConverter.Abstractions
{
    public class JsonMapResult<TData>
    {
        public string MapJsonStr { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }

        public IList<JsonMapInfo> PropertyInfos { get; set; } = new List<JsonMapInfo>();

        public TData? MapData
        {
            get
            {
                if (string.IsNullOrEmpty(MapJsonStr) || _buildDataFunc == null)
                {
                    return default(TData);
                }

                return _buildDataFunc(MapJsonStr);
            }
        }

        private Func<string, TData?>? _buildDataFunc { get; }

        public JsonMapResult(Func<string, TData?>? buildDtaFunc)
        {
            _buildDataFunc = buildDtaFunc;
        }
    }
}
