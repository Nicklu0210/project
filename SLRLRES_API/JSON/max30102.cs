namespace SLRLRES_API.JSON
{
    public class max30102_input
    {
        public string? account { get; set; }

        public long Mb { get; set; } 

        public int? heart { get; set; }

        public float? oxygen { get; set; }

        public string? key { get; set; }

    }
    public class max30102_Return
    {
        /// <summary>
        /// 響應代碼
        /// </summary>
        public int code { get; set; } = 200;
        /// <summary>
        /// 信息
        /// </summary>
        public string? msg { get; set; }
        /// <summary>
        /// 用戶數據
        /// </summary>
        public List<max30102_input>? Data { get; set; }
    }
}