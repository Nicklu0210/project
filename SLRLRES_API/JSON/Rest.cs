namespace SLRLRES_API.JSON
{

    public class Reset_Input
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public string? account { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        public string? password { get; set; }
        /// <summary>
        /// 郵箱
        /// </summary>
        public string? mail { get; set; }
    }
    public class Reset_Return
    {
        /// <summary>
        /// 響應代碼
        /// </summary>
        public int code { get; set; } = 200;
        /// <summary>
        /// 信息
        /// </summary>
        public string? msg { get; set; }
    }
}
