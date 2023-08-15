using MySql.Data.MySqlClient;
using System.Reflection;
using static SLRLRES_API.MODE.MYSQL;

namespace SLRLRES_API.DB
{
    public class UserTable
    {

        public class User
        {
            public ProcessingDatabase _DB;

            /// <summary>
            /// 初始化
            /// </summary>
            public User(ProcessingDatabase DB)
            {
                _DB = DB;//接收处理的数据库
            }

            /// <summary>
            /// 初始化表
            /// </summary>
            public MySqlCommand? InitializationTable(bool Single = true)
            {
                string TempString = @"CREATE TABLE `slrlres`.`user`  (
                                                                    `id` int NOT NULL AUTO_INCREMENT COMMENT '主鍵',
                                                                    `group` int NULL COMMENT '分組',
                                                                    `account` char(7) NOT NULL COMMENT '账户',
                                                                    `password` varchar(255) NOT NULL COMMENT '密碼',
                                                                    `name` varchar(255) NOT NULL COMMENT '名稱',
                                                                    `mail` varchar(255) NULL COMMENT '郵箱',
                                                                    `phone` varchar(15) NULL COMMENT '電話',
                                                                    `birthday` date NULL COMMENT '生日',
                                                                    `work_number` varchar(20) NULL COMMENT '工作编号',
                                                                    `level` varchar(255) NULL COMMENT '教育程度',
                                                                    `department` varchar(255) NULL COMMENT '部門',
                                                                    `duration` float NULL COMMENT '患病期數',
                                                                    `sex` varchar(255) NULL COMMENT '性別',
                                                                    `medicine_name` varchar(255) NULL COMMENT '服用藥物名稱',
                                                                     PRIMARY KEY (`id`)
                                                                     );";
                if (Single)
                    return _DB.CommandSingle(TempString);
                else
                    return _DB.Command(TempString);
            }

            /// <summary>
            /// 获取用户真实数量
            /// </summary>
            /// <returns></returns>
            public long GetTotalNumber()
            {
                long ReturnData = -1;
                try
                {
                    _DB.Open();
                    ReturnData = (long)_DB.Command(@"SELECT COUNT(id) FROM user")!.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    _DB.Close();
                }
                return ReturnData;
            }

            /// <summary>
            /// 以用户名或邮箱或状态搜索
            /// </summary>
            /// <param name="Value">用户名 或 邮箱 或 状态</param>
            /// <returns>用户内容</returns>
            public List<Definition?>? GetData(string Value)
            {
                MySqlDataReader ReturnDataTemp;
                List<Definition?> ReturnData = new();
                try
                {
                    _DB.Open();
                    ReturnDataTemp = _DB.Command($"SELECT *FROM user WHERE account='{Value}' OR mail='{Value}'")!.ExecuteReader();
                    while (ReturnDataTemp.Read())
                    {
                        ReturnData.Add(new()
                        {
                            id = ReturnDataTemp.GetInt32("id"),
                            group = ReturnDataTemp.GetInt32("group"),
                            account = ReturnDataTemp.GetString("account"),
                            password = ReturnDataTemp.GetString("password"),
                            name = ReturnDataTemp.GetString("name"),
                            mail = ReturnDataTemp.GetString("mail"),
                            phone = ReturnDataTemp.GetString("phone"),
                            birthday = ReturnDataTemp.GetDateTime("birthday"),
                            work_number = ReturnDataTemp.GetString("work_number"),
                            level = ReturnDataTemp.GetString("level"),
                            department = ReturnDataTemp.GetString("department"),
                            duration = ReturnDataTemp.GetFloat("duration"),
                            sex = ReturnDataTemp.GetString("sex"),
                            medicine_name = ReturnDataTemp.GetString("medicine_name"),
                        });
                    }
                    ReturnDataTemp.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                finally
                {
                    _DB.Close();
                }
                return ReturnData;
            }

            /// <summary>
            /// 以用ID搜索
            /// </summary>
            /// <param name="ID">用户ID</param>
            /// <returns>用户内容</returns>
            public List<Definition?>? GetData(int ID)
            {
                MySqlDataReader ReturnDataTemp;
                List<Definition?> ReturnData = new();
                try
                {
                    _DB.Open();
                    ReturnDataTemp = _DB.Command($"SELECT *FROM user WHERE id='{ID}'")!.ExecuteReader();
                    while (ReturnDataTemp.Read())
                    {
                        ReturnData.Add(new()
                        {
                            id = ReturnDataTemp.GetInt32("id"),
                            group = ReturnDataTemp.GetInt32("group"),
                            account = ReturnDataTemp.GetString("account"),
                            password = ReturnDataTemp.GetString("password"),
                            name = ReturnDataTemp.GetString("name"),
                            mail = ReturnDataTemp.GetString("mail"),
                            phone = ReturnDataTemp.GetString("phone"),
                            birthday = ReturnDataTemp.GetDateTime("birthday"),
                            work_number = ReturnDataTemp.GetString("work_number"),
                            level = ReturnDataTemp.GetString("level"),
                            department = ReturnDataTemp.GetString("department"),
                            duration = ReturnDataTemp.GetFloat("duration"),
                            sex = ReturnDataTemp.GetString("sex"),
                            medicine_name = ReturnDataTemp.GetString("medicine_name"),
                        });
                    }
                    ReturnDataTemp.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                finally
                {
                    _DB.Close();
                }
                return ReturnData;
            }
        }

        public class Definition
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
        }
    }
}


/*
 CREATE TABLE `slrlres`.`user`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '主鍵',
  `group` int NULL COMMENT '分組',
  `account` char(7) NOT NULL COMMENT '账户',
  `password` varchar(255) NOT NULL COMMENT '密碼',
  `name` varchar(255) NOT NULL COMMENT '名稱',
  `mail` varchar(255) NULL COMMENT '郵箱',
  `phone` varchar(15) NULL COMMENT '電話',
  `birthday` datetime NULL COMMENT '生日',
  `work_number` varchar(20) NULL COMMENT '工作编号',
  `level` varchar(255) NULL COMMENT '教育程度',
  `department` varchar(255) NULL COMMENT '部門',
  `duration` float NULL COMMENT '患病期數',
  `gender` varchar(255) NULL COMMENT '性別',
  PRIMARY KEY (`id`)
);
 */
