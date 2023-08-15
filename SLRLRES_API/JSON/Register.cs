namespace SLRLRES_API.JSON
{
    public class Register
    {
        /// <summary>
        /// 註冊傳入
        /// </summary>
        public class Register_Input
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
            /// 名稱
            /// </summary>
            //public string? name { get; set; }
            /// <summary>
            /// 郵箱
            /// </summary>
            public string? mail { get; set; }
            /// <summary>
            /// 電話
            /// </summary>
            //public string? phone { get; set; }
            /// <summary>
            /// 生日
            /// </summary>
           // public DateTime? birthday { get; set; }
            /// <summary>
            /// 醫師工號
            /// </summary>
            //public string? work_number { get; set; }
            /// <summary>
            /// 教育程度
            /// </summary>
            //public string? level { get; set; }
            /// <summary>
            /// 部門
            /// </summary>
            //public string? department { get; set; } 
            /// <summary>
            /// 患病期數
            /// </summary>
            //public float? duration { get; set; }
            /// <summary>
            /// 性別
            /// </summary>
            //public string? gender { get; set; }
            /// <summary>
            /// 用藥名稱
            /// </summary>
            //public string? drug_name { get; set; }

            /// <summary>
            /// 默認為用戶
            /// </summary>
            //public int group { get; set; }// = 2;
            /// <summary>
            /// 医师注册邀请码
            /// </summary>
            public string? invitation { get; set; }
        }
        /// <summary>
        /// 註冊傳出
        /// </summary>
        public class Register_Return
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
}
