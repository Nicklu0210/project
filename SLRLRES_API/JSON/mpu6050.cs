namespace SLRLRES_API.JSON
{
    public class mpu6050_input
    {
        public string? account { get; set; }

        public long Mb { get; set; }    

        public float? X_gyro { get; set; }

        public float? Y_gyro { get; set; }

        public float? Z_gyro { get; set; }

        public string? key { get; set;}

    }
    public class mpu6050_Return
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
        /// 用戶數據
        /// </summary>
        public List<mpu6050_input>? Data { get; set; }
    }
}
