using LoongEgg.LoongLogger;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SLRLRES_API.DB;
using SLRLRES_API.JSON;
using System.Text.RegularExpressions;
using Yitter.IdGenerator;

using static SLRLRES_API.JSON.Longin;
using static SLRLRES_API.JSON.Register;
using static SLRLRES_API.MODE.MYSQL;




namespace SLRLRES_API.Controllers
{
    [ApiController]
    [Route("User")]
    public class User_API : ControllerBase
    {

        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// 通常用於在ASP.NET Core應用程式中存取HTTP請求的相關資訊，例如用戶身份、請求資訊等等。
        /// </summary>
        public User_API(IHttpContextAccessor _httpContextAccessor)
        {
            httpContextAccessor = _httpContextAccessor;
        }

        /// <summary>
        /// 創建一個新的ProcessingDatabase物件，並將它存儲在靜態欄位_DB中
        /// </summary>
        static ProcessingDatabase _DB = new(new());

        static UserTable.User UserOBJ = new(_DB);
        /// <summary>
        /// 創建一個新的靜態的字典物件，用來存儲使用者登入相關的資料
        /// </summary>
        public static Dictionary<string, UserTable.Definition> LonginUserList = new();  //string存取key

        /// <summary>
        /// 偞离SQL砓
        /// </summary>
        /// <param name="Database"></param>
        //SetData
        public static void SetData(UserTable.User Database)
        {
            _DB = Database._DB;
            UserOBJ = Database;
        }

        /// <summary>
        /// 帳戶註冊接口
        /// </summary>
        [HttpPost("Register")]
        public Register_Return Register(Register_Input Data)
        {
            Logger.WriteInfor($"帳戶註冊接口請求(Post)IP {httpContextAccessor.HttpContext?.Connection.RemoteIpAddress}");
            //MySqlDataReader ReturnDataTemp;
            Register_Return ReturnData = new();
            try
            {
                if (Data.mail != null)//信箱驗證判斷式
                {
                    Regex regex = new Regex(@"^(\w)+(\.\w)*@(\w)+((\.\w+)+)$");//電子信箱正則表達式
                    if (!regex.IsMatch(Data.mail))
                    {
                        throw new Exception("?電子信箱格式不符!");
                    }
                    if (UserOBJ.GetData(Data.mail!)?.Count != 0)//檢查電子信箱是否為空
                    {
                        throw new Exception("請輸入電子信箱!");
                    }
                }
                if (Data.password == null || Data.password == "")//判斷密碼是否為空
                {
                    throw new Exception("請輸入密碼!");
                }
                if (Data.account != null)//身分證帳號判斷式
                {
                    Regex regex = new Regex(@"^[A-Z][0-9]{4}$");//身分證帳號正則表達式(字母+後4碼)    
                    if (!regex.IsMatch(Data.account))
                    {
                        throw new Exception("身分證帳號格式不符!");
                    }
                    if (UserOBJ.GetData(Data.account!)?.Count != 0)//檢查身分證帳號是否為空
                    {
                        throw new Exception("請輸入身分證帳號!");
                    }
                }

                /*if (UserOBJ.GetData(Data.account!)?.Count != 0)
                {
                    throw new Exception("?眒湔婓!");
                }*/
                _DB.Open();//mysql連線開啟
                if (Data.invitation == "400")//invitation 輸入邀請碼400為醫師
                {
                    ReturnData.msg = ("成功註冊醫師帳號!");
                    _DB.Command(@$"INSERT INTO user (account,password,mail,`group`) VALUES ('{Data.account}','{Data.password}','{Data.mail}',1);");//成功註冊醫師帳號group為1
                }
                else if (Data.invitation == "200")//invitation 輸入邀請碼200為後台管理員
                {
                    ReturnData.msg = ("成功註冊後台管理員帳號!");
                    _DB.Command(@$"INSERT INTO user (account,password,mail,`group`) VALUES ('{Data.account}','{Data.password}','{Data.mail}',0);");//成功註冊後臺管理員帳號group為0
                }
                else
                {
                    ReturnData.msg = ("成功註冊使用者帳號!");//invitation不輸入邀請碼為一般患者
                    _DB.Command(@$"INSERT INTO user (account,password,mail,`group`) VALUES ('{Data.account}','{Data.password}','{Data.mail}',2);");//成功註冊使用者帳號group為1
                }
            }

            catch (Exception ex)
            {
                Logger.WriteError(ex.Message);
                ReturnData.code = 400; //出現錯誤"400"
                ReturnData.msg = ex.Message;
                return ReturnData;
            }
            finally//上述程式執行完成後一定會執行這段
            {
                _DB?.Close();//SQL關閉連線
            }
            return ReturnData;
        }

        /// <summary>
        /// 登入接口
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public Longin_Return Login(Longin_Input Data)
        {
            Logger.WriteInfor($"登入連線請求(Post)接口IP {httpContextAccessor.HttpContext?.Connection.RemoteIpAddress}");
            MySqlDataReader ReturnDataTemp;
            Longin_Return ReturnData = new();
            try
            {
                if (Data.password == null || Data.password == "")
                {
                    throw new Exception("請輸入帳號及密碼!");
                }

                _DB.Open();//SQL連線開啟
                ReturnDataTemp = _DB.Command($"SELECT *FROM user WHERE (account='{Data.account}') AND password='{Data.password}'")!.ExecuteReader();
                ReturnDataTemp.Read();
                ReturnData.data = new()
                {
                    id = ReturnDataTemp.GetInt32("id"),
                    group = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("group")) ? null : (int?)ReturnDataTemp.GetInt32("group"),
                    account = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("account")) ? null : ReturnDataTemp.GetString("account"),
                    password = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("password")) ? null : ReturnDataTemp.GetString("password"),
                    name = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("name")) ? null : ReturnDataTemp.GetString("name"),
                    mail = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("mail")) ? null : ReturnDataTemp.GetString("mail"),
                    phone = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("phone")) ? null : ReturnDataTemp.GetString("phone"),
                    birthday = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("birthday")) ? null : (DateTime?)ReturnDataTemp.GetDateTime("birthday"),
                    work_number = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("work_number")) ? null : ReturnDataTemp.GetString("work_number"),
                    level = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("level")) ? null : ReturnDataTemp.GetString("level"),
                    department = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("department")) ? null : ReturnDataTemp.GetString("department"),
                    duration = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("duration")) ? null : (float?)ReturnDataTemp.GetFloat("duration"),
                    sex = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("sex")) ? null : ReturnDataTemp.GetString("sex"),
                    medicine_name = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("medicine_name")) ? null : ReturnDataTemp.GetString("medicine_name")
                    /*  id = ReturnDataTemp.GetInt32("id"),
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
                      gender = ReturnDataTemp.GetString("gender"),
                      drug_name = ReturnDataTemp.GetString("drug_name") */
                };

                ReturnData.key = YitIdHelper.NextId().ToString();
                LonginUserList.Add(ReturnData.key, ReturnData.data);
                ReturnDataTemp.Close();



            }
            catch (Exception ex)
            {
                Logger.WriteError(ex.Message);
                ReturnData.code = 400;//出現錯誤"400"
                ReturnData.msg = ("帳號或密碼錯誤");///ex.Message;
            }
            finally//上述程式執行完成後一定會執行這段
            {
                _DB.Close();//SQL關閉連線
            }

            return ReturnData;
        }

        /// <summary>
        /// 重置使用者密碼請求街口
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPut("Reset")]
        public Reset_Return Rreset(Reset_Input Data)
        {
            Logger.WriteInfor($"重置使用者密碼請求連線(Put)接口IP {httpContextAccessor.HttpContext?.Connection.RemoteIpAddress}");
            //MySqlDataReader ReturnDataTemp;
            Reset_Return ReturnData = new();
            try
            {
                if (Data.mail != null)//檢查電子信箱是否為空
                {
                    Regex regex = new Regex(@"^(\w)+(\.\w)*@(\w)+((\.\w+)+)$");
                    if (!regex.IsMatch(Data.mail))
                    {
                        throw new Exception("電子信箱格式不符!");
                    }
                    if (Data.password == null || Data.password == "")//檢查密碼是否為空
                    {
                        throw new Exception("請輸入新密碼!");
                    }
                    if ((UserOBJ.GetData(Data.mail!)?.Count != 0) && (UserOBJ.GetData(Data.account!)?.Count != 0))
                    {
                        _DB.Open();//mysql連線開啟
                        _DB.Command(@$"UPDATE user SET password ='{Data.password}' WHERE (account='{Data.account}') AND mail='{Data.mail}'");
                    }
                    else
                    {
                        throw new Exception("帳號錯誤");
                    }
                }
                else if ((Data.mail == null))
                {
                    throw new Exception("請輸入電子信箱");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex.Message);
                ReturnData.code = 400; //出現錯誤"400"
                ReturnData.msg = ex.Message;
                return ReturnData;
            }
            finally//上述程式執行完成後一定會執行這段
            {
                _DB?.Close();//SQL關閉連線
            }
            return ReturnData;
        }

        /// <summary>
        /// 取得使用者資料
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Objectives"></param>
        /// <returns></returns>
        [HttpGet("GetUserList")]
        public SLRLRES_API.JSON.GetUserList.GetUserList_Return GetUserList(string? obj, string? Objectives)
        {
            Logger.WriteInfor($"取得使用者資料請求連線(Get)接口IP {httpContextAccessor.HttpContext?.Connection.RemoteIpAddress}");
            MySqlDataReader ReturnDataTemp;
            SLRLRES_API.JSON.GetUserList.GetUserList_Return ReturnData = new();
            ReturnData.data = new();
            try
            {
                if (obj != null && Objectives == null)
                    throw new Exception("Objectives為空");

                _DB.Open(); //mysql連線開啟
                if (obj == null)
                    ReturnDataTemp = _DB.Command(@"SELECT * FROM user")!.ExecuteReader();
                else
                    ReturnDataTemp = _DB.Command($"SELECT * FROM user WHERE {obj}='{Objectives}'")!.ExecuteReader();

                while (ReturnDataTemp.Read())
                {
                    ReturnData.data.Add(new UserTable.Definition()
                    {
                        id = ReturnDataTemp.GetInt32("id"),
                        group = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("group")) ? null : (int?)ReturnDataTemp.GetInt32("group"),
                        account = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("account")) ? null : ReturnDataTemp.GetString("account"),
                        password = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("password")) ? null : ReturnDataTemp.GetString("password"),
                        name = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("name")) ? null : ReturnDataTemp.GetString("name"),
                        mail = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("mail")) ? null : ReturnDataTemp.GetString("mail"),
                        phone = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("phone")) ? null : ReturnDataTemp.GetString("phone"),
                        birthday = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("birthday")) ? null : (DateTime?)ReturnDataTemp.GetDateTime("birthday"),
                        work_number = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("work_number")) ? null : ReturnDataTemp.GetString("work_number"),
                        level = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("level")) ? null : ReturnDataTemp.GetString("level"),
                        department = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("department")) ? null : ReturnDataTemp.GetString("department"),
                        duration = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("duration")) ? null : (float?)ReturnDataTemp.GetFloat("duration"),
                        sex = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("sex")) ? null : ReturnDataTemp.GetString("sex"),
                        medicine_name = ReturnDataTemp.IsDBNull(ReturnDataTemp.GetOrdinal("medicine_name")) ? null : ReturnDataTemp.GetString("medicine_name")
                    });
                }
                ReturnDataTemp.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex.Message);
            }
            finally//上述程式執行完成後一定會執行這段
            {
                _DB.Close();//SQL關閉連線
            }
            return ReturnData;
        }

        /// <summary>
        /// 更新使用者個人資料
        /// </summary>
        [HttpPut("Updateuserdata")]
        public Updatedata_Return Updatedata(Updatedata_input Data)
        {
            Logger.WriteInfor($"更新使用者個人資料連線請求(Put)接口IP {httpContextAccessor.HttpContext?.Connection.RemoteIpAddress}");
            UserTable.Definition Longin_Return = new();
            //MySqlDataReader ReturnDataTemp;
            Updatedata_Return ReturnData = new();
            try
            {
                if (LonginUserList.ContainsKey(Data.key!))//查字典看有沒有key
                {
                    Longin_Return = LonginUserList[Data.key!];
                    _DB.Open();//mysql連線開啟
                               // Longin_Return.account;
                    _DB.Command(@$"UPDATE user SET name ='{Data.name}', phone ='{Data.phone}', birthday ='{Data.birthday}', work_number ='{Data.work_number}', level ='{Data.level}', department ='{Data.department}', duration ='{Data.duration}', sex ='{Data.sex}', medicine_name ='{Data.medicine_name}'WHERE (account='{Longin_Return.account}')");

                }
                else
                {
                    throw new Exception("沒登入?");
                }
            }
            catch (Exception ex)
            {

                Logger.WriteError(ex.Message);
                ReturnData.code = 400; //出現錯誤"400"
                ReturnData.msg = ex.Message;
                return ReturnData;
            }
            finally//上述程式執行完成後一定會執行這段
            {
                _DB?.Close();//SQL關閉連線
            }
            return ReturnData;
        }


        /// <summary>
        /// 使用者帳號註銷
        /// </summary>
        [HttpPut("Userdelete")]
        public Userdelete_Return Userdelete(Userdelete_input Data)
        {
            Logger.WriteInfor($"使用者帳號註銷連線請求(Put)接口IP {httpContextAccessor.HttpContext?.Connection.RemoteIpAddress}");
            UserTable.Definition Longin_Return = new();
            //MySqlDataReader ReturnDataTemp;
            Userdelete_Return ReturnData = new();
            try
            {
                if (LonginUserList.ContainsKey(Data.key!))//查字典看有沒有key
                {
                    Longin_Return = LonginUserList[Data.key!];
                    _DB.Open();//mysql連線開啟
                    _DB.Command(@$"Delete from user WHERE account='{Longin_Return.account}'");
                    _DB.Command(@$"Delete from max30102 WHERE account='{Longin_Return.account}'");
                    _DB.Command(@$"Delete from mpu6050 WHERE account='{Longin_Return.account}'");
                }
                else
                {
                    throw new Exception("無法確認登入");
                }
            }
            catch (Exception ex)
            {

                Logger.WriteError(ex.Message);
                ReturnData.code = 400; //?躇徨赩眊笢衄嶒悷麼笭恚枑尨測徨"400"
                ReturnData.msg = ex.Message;
                return ReturnData;
            }
            finally//上述程式執行完成後一定會執行這段
            {
                _DB?.Close();//SQL關閉連線
            }
            return ReturnData;
        }




        /// <summary>
        /// 新增max30102心律血氧數據
        /// </summary>
        [HttpPost("addmax30102data")]
        public max30102_Return max30102(max30102_input Data)
        {
            Logger.WriteInfor($"新增max30102心律血氧數據連線請求(Post)接口IP {httpContextAccessor.HttpContext?.Connection.RemoteIpAddress}");
            UserTable.Definition Longin_Return = new();
            //MySqlDataReader ReturnDataTemp;
            max30102_Return ReturnData = new();
            try
            {

                if (LonginUserList.ContainsKey(Data.key!))//查字典看有沒有key
                {
                    Longin_Return = LonginUserList[Data.key!];
                    _DB.Open();//mysql連線開啟
                    _DB.Command(Sql: @$"INSERT INTO max30102 (account,Mb,`heart`,`oxygen`) VALUES ('{Longin_Return.account}','{DateTimeOffset.Now.ToUnixTimeSeconds()}','{Data.heart}','{Data.oxygen}');");

                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex.Message);
                ReturnData.code = 400; 
                ReturnData.msg = ex.Message;
                return ReturnData;
            }
            finally//上述程式執行完成後一定會執行這段
            {
                _DB?.Close();//SQL關閉連線
            }
            return ReturnData;
        }



        /// <summary>
        /// 新增mpu6050運動數據
        /// </summary>
        [HttpPost("addmpu6050data")]
        public mpu6050_Return mpu6050(mpu6050_input Data)
        {
            Logger.WriteInfor($"新增mpu6050運動數據連線請求(Post)接口IP {httpContextAccessor.HttpContext?.Connection.RemoteIpAddress}");
            UserTable.Definition Longin_Return = new();
            //MySqlDataReader ReturnDataTemp;
            mpu6050_Return ReturnData = new();
            try
            {
                if (LonginUserList.ContainsKey(Data.key!))//查字典看有沒有key
                {
                    Longin_Return = LonginUserList[Data.key!];
                    _DB.Open();//mysql連線開啟
                    _DB.Command(Sql: @$"INSERT INTO mpu6050 (account,Mb,`x_gyro`,`y_gyro`,`z_gyro`) VALUES ('{Longin_Return.account}','{DateTimeOffset.Now.ToUnixTimeSeconds()}','{Data.X_gyro}','{Data.Y_gyro}','{Data.Z_gyro}');"); //

                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex.Message);
                ReturnData.code = 400; //出現錯誤"400"
                ReturnData.msg = ex.Message;
                return ReturnData;
            }
            finally//上述程式執行完成後一定會執行這段
            {
                _DB?.Close();//SQL關閉連線
            }
            return ReturnData;
        }


        /// <summary>
        /// 取得mpu6050運動數據
        /// </summary>
        [HttpGet("getmpu6050data")]
        public mpu6050_Return getmpu6050(string account)
        {
            //UserTable.Definition Longin_Return = new();
            //MySqlDataReader ReturnDataTemp;
            mpu6050_Return ReturnData = new();
            //
            try
            {
                MySqlDataReader ReturnDataTemp;
                _DB.Open();//mysql連線開啟
                ReturnDataTemp = _DB.Command($"SELECT * FROM mpu6050 WHERE (account='{account}');")!.ExecuteReader();
                ReturnData.Data = new();
                while (ReturnDataTemp.Read())
                {
                    ReturnData.Data.Add(new()
                    {
                        account = ReturnDataTemp.GetString("account"),
                        Mb = ReturnDataTemp.GetInt64("Mb"),
                        X_gyro = ReturnDataTemp.GetFloat("x_gyro"),
                        Y_gyro = ReturnDataTemp.GetFloat("y_gyro"),
                        Z_gyro = ReturnDataTemp.GetFloat("z_gyro"),
                    });
                }
                ReturnDataTemp.Close();
                if (ReturnData.Data.Count == 0)
                {
                    ReturnData.Data = null;
                    throw new Exception("脤揃蹋!");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex.Message);
                ReturnData.code = 400; //出現錯誤"400"
                ReturnData.msg = ex.Message;
                return ReturnData;
            }
            finally//上述程式執行完成後一定會執行這段
            {
                _DB?.Close();//SQL關閉連線
            }
            return ReturnData;
        }





        /// <summary>
        /// 取得max30102心律血氧數據
        /// </summary>
        [HttpGet("getmax30102data")]//取得max30102數據
        public max30102_Return getmax30102(string account)
        {
            //UserTable.Definition Longin_Return = new();
            //MySqlDataReader ReturnDataTemp;
            max30102_Return ReturnData = new();
            try
            {
                MySqlDataReader ReturnDataTemp;
                _DB.Open();//mysql連線開啟
                ReturnDataTemp = _DB.Command($"SELECT * FROM max30102 WHERE (account='{account}');")!.ExecuteReader();
                ReturnData.Data = new();
                while (ReturnDataTemp.Read())
                {
                    ReturnData.Data.Add(new()
                    {
                        account = ReturnDataTemp.GetString("account"),
                        Mb = ReturnDataTemp.GetInt64("Mb"),
                        heart = ReturnDataTemp.GetInt32("heart"),
                        oxygen = ReturnDataTemp.GetFloat("oxygen"),

                    });
                }
                ReturnDataTemp.Close();
                if (ReturnData.Data.Count == 0)
                {
                    ReturnData.Data = null;
                    throw new Exception("脤揃蹋!");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex.Message);
                ReturnData.code = 400; //出現錯誤"400"
                ReturnData.msg = ex.Message;
                return ReturnData;
            }
            finally//上述程式執行完成後一定會執行這段
            {
                _DB?.Close();//SQL關閉連線
            }
            return ReturnData;
        }









        





    }
}


