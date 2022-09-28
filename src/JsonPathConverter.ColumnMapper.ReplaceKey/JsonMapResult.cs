namespace JsonPathConverter.ColumnMapper.ReplaceKey
{
    internal class JsonMapResult<TData>
    {
        public string MapJsonStr { get; set; } = string.Empty;

        public IList<JsonMapInfo> PropertyInfos { get; set; } = new List<JsonMapInfo>();

        public TData? MapData
        {
            get
            {
                if (_mapData != null)
                {
                    return _mapData;
                }

                if (string.IsNullOrEmpty(MapJsonStr) || _buildDataFunc == null)
                {
                    return default(TData);
                }

                return _buildDataFunc(MapJsonStr);
            }
            set
            {
                _mapData = value;
            }
        }

        private TData? _mapData;

        private Func<string, TData?>? _buildDataFunc { get; }

        public JsonMapResult(Func<string, TData?>? buildDtaFunc)
        {
            _buildDataFunc = buildDtaFunc;
        }

        public JsonMapResult(TData? mapData)
        {
            MapData = mapData;
        }
    }
}
