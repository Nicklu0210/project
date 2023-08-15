using System.Data;
using LoongEgg.LoongLogger;
using MySql.Data.MySqlClient;

namespace SLRLRES_API.MODE
{
    public class MYSQL
    {
        /// <summary>
        /// 数据库处理
        /// </summary>
        public class ProcessingDatabase
        {
            /// <summary>
            /// 连接信息
            /// </summary>
            public ConnData _ConnData { get; }

            /// <summary>
            /// 连接字符串
            /// </summary>
            public string _ConnStr { get; }

            /// <summary>
            /// 连接
            /// </summary>
            public MySqlConnection _Conn { get; }

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="ConnectionData"></param>
            public ProcessingDatabase(ConnData ConnData)
            {
                _ConnData = ConnData;//初始化连接信息
                _ConnStr = ConnDataToStr(ConnData);//初始化连接字符串
                _Conn = new MySqlConnection(_ConnStr);//初始化连接
                ConneTest();//连接测试
            }

            /// <summary>
            /// 连接信息转连接字符串
            /// </summary>
            /// <param name="ConnData">连接信息</param>
            /// <returns>连接信息字符串</returns>
            public static string ConnDataToStr(ConnData ConnData)
            {
                return $"server = {ConnData.Server}; user = {ConnData.User}; database = {ConnData.Database}; port = {ConnData.Port}; password = {ConnData.Password}";
            }

            /// <summary>
            /// 连接数据库测试
            /// </summary>
            public bool ConneTest()
            {
                bool ReturnData = Open();
                if (ReturnData)
                {
                    Logger.WriteInfor($"连接数据库成功! 服务器:{_ConnData.Server},数据库:{_ConnData.Database}");
                }
                Close();
                return ReturnData;
            }

            /// <summary>
            /// Sql命令执行
            /// </summary>
            /// <param name="Sql">Sql命令</param>
            /// <returns>执行结果</returns>
            public MySqlCommand? Command(string Sql)
            {
                MySqlCommand Cmd;
                try
                {
                    Cmd = new MySqlCommand(Sql, _Conn);
                    Logger.WriteInfor($"执行命令成功! 影响行数:{Cmd.ExecuteNonQuery()} 命令:{Sql}");
                }
                catch (Exception ex)
                {
                    Logger.WriteError($"执行命令失败! 因为: {ex.Message} 命令:{Sql}");
                    return null;
                }
                return Cmd;
            }

            /// <summary>
            /// Sql命令执行(单次)
            /// </summary>
            /// <param name="Sql">Sql命令</param>
            /// <returns>执行结果</returns>
            public MySqlCommand? CommandSingle(string Sql)
            {
                MySqlCommand Cmd;
                try
                {
                    Open();
                    Cmd = new MySqlCommand(Sql, _Conn);
                    Logger.WriteInfor($"执行命令成功! 影响行数:{Cmd.ExecuteNonQuery()} 命令:{Sql}");
                }
                catch (Exception ex)
                {
                    Logger.WriteError($"执行命令失败! 因为: {ex.Message} 命令:{Sql}");
                    return null;
                }
                finally
                {
                    Close();
                }
                return Cmd;
            }

            /// <summary>
            /// 打开连接
            /// </summary>
            public bool Open()
            {
                try
                {
                    _Conn.Open();
                }
                catch (Exception ex)//报错处理
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 关闭连接
            /// </summary>
            public void Close()
            {
                _Conn.Close();
            }

            /// <summary>
            /// 获取状态
            /// </summary>
            /// <returns>状态</returns>
            public ConnectionState Status()
            {
                return _Conn.State;
            }

            //internal void Command(string spl)
           // {
            //    throw new NotImplementedException();
         //   }

            /// <summary>
            /// 连接信息
            /// </summary>
            public class ConnData
            {
                /// <summary>
                /// 服务器地址
                /// </summary>
                public string Server { set; get; } = "localhost";

                /// <summary>
                /// 服务器端口
                /// </summary>
                public int Port { set; get; } = 3306;

                /// <summary>
                /// 数据库账号
                /// </summary>
                public string User { set; get; } = "root";

                /// <summary>
                /// 数据库密码
                /// </summary>
                public string? Password { set; get; }

                /// <summary>
                /// 数据库名
                /// </summary>
                public string? Database { set; get; }
            }
        }
    }
}