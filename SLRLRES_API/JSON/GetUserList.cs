using SLRLRES_API.DB;

namespace SLRLRES_API.JSON
{
    public class GetUserList
    {
        public class GetUserList_Return
        {
            public int code { get; set; } = 200;
            public string? msg { get; set; }
            public List<UserTable.Definition>? data { get; set; }
        }
    }
}
