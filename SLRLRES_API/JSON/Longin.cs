using SLRLRES_API.DB;

namespace SLRLRES_API.JSON
{
    public class Longin
    {
        /// <summary>
        /// 登入傳入
        /// </summary>
        public class Longin_Input
        {
            /// <summary>
            /// 帳號
            /// </summary>
            public string? account { get; set; }
            /// <summary>
            /// 密碼
            /// </summary>
            public string? password { get; set; }
        }

        /// <summary>
        /// 登入傳出
        /// </summary>
        public class Longin_Return
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
            /// 用戶信息
            /// </summary>
            public UserTable.Definition? data {get;set;}
            /// <summary>
            /// 生份驗證(暫)
            /// </summary>
            public string? key { get; set; }
        }
    }
}
