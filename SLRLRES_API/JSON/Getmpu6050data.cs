
using SLRLRES_API.DB;

namespace SLRLRES_API.JSON
{
    public class Getnpu6050List
    {
       
    }
    public class GetUserList_Return
    {
        public int code { get; set; }
        public string? msg { get; set; }
        public List<UserTable.Definition>? data { get; set; }
    }
}
