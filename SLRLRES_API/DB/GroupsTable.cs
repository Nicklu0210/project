using MySql.Data.MySqlClient;
using static SLRLRES_API.MODE.MYSQL;

namespace SLRLRES_API.DB
{
    public class GroupTable
    {
        public class Group
        {
            public ProcessingDatabase _DB;

            /// <summary>
            /// 初始化
            /// </summary>
            public Group(ProcessingDatabase DB)
            {
                _DB = DB;//接收处理的数据库
            }

            /// <summary>
            /// 初始化表
            /// </summary>
            public MySqlCommand? InitializationTable(bool Single = true)
            {
                string TempString = @"CREATE TABLE `slrlres`.`group`  (
                                                                        `id` int NOT NULL AUTO_INCREMENT COMMENT '主鍵',
                                                                        `name` varchar(255) NOT NULL COMMENT '分組名',
                                                                        `weight` int NOT NULL COMMENT '權重',
                                                                         PRIMARY KEY (`id`)
                                                                        );";
                if (Single)
                    return _DB.CommandSingle(TempString);
                else
                    return _DB.Command(TempString);
            }

        }
    }
}

/*
 CREATE TABLE `slrlres`.`group`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '主鍵',
  `name` varchar(255) NOT NULL COMMENT '分組名',
  `weight` int NOT NULL COMMENT '權重',
  PRIMARY KEY (`id`)
);
 */