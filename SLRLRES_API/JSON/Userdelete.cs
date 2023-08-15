namespace SLRLRES_API.JSON
{
    public class Userdelete_input
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 分组
        /// </summary>
        public int? group { get; set; } = null;

        /// <summary>
        /// 用户名
        /// </summary>
        public string? account { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string? password { get; set; }
        public string? name { get; set; }

        /// <summary>
        /// 用户邮件
        /// </summary>
        public string? mail { get; set; }


        /// <summary>
        /// 生日
        /// </summary>
        public string? phone { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? birthday { get; set; }

        /// <summary>
        /// 工作编号
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
        /// 分組
        /// </summary>
        public string? sex { get; set; }
        public string? medicine_name { get; set; }
        public string? key { get; set; }
    }
    public class Userdelete_Return
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
