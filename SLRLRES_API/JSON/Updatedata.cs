namespace SLRLRES_API.JSON
{
    public class Updatedata_input
    {
        //public string? mail { get; set; }
        /// <summary>
        /// 帳號
        /// </summary>
        public string? account { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? name { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        public string? phone { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
         public DateOnly? birthday { get; set; }
        /// <summary>
        /// 醫師工號
        /// </summary>
        public string? work_number { get; set; }
        /// <summary>
        /// 教育程度
        /// </summary>
        public string? level { get; set; }
        /// <summary>
        /// 部門
        /// </summary>
        public string? department { get; set; } 
        /// <summary>
        /// 患病期數
        /// </summary>
        public float? duration { get; set; }
        /// <summary>
        /// 性別
        /// </summary>
        public string? sex { get; set; }
        /// <summary>
        /// 用藥名稱
        /// </summary>
        public string? medicine_name { get; set; }

        /// <summary>
        /// KEY
        /// </summary>
        public string? key { get; set; }
    }
    public class Updatedata_Return
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
