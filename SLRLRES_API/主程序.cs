using LoongEgg.LoongLogger;
using SLRLRES_API.Controllers;
using SLRLRES_API.DB;
using Yitter.IdGenerator;
using static SLRLRES_API.MODE.MYSQL;

Logger.Enable(LoggerType.Console | LoggerType.Debug | LoggerType.File, LoggerLevel.Debug);//蛁聊Log祩滲杅

// 斐膘 IdGeneratorOptions 勤砓ㄛ褫婓凳婖滲杅笢怀 WorkerIdㄩ
var options = new IdGeneratorOptions();
// options.WorkerIdBitLength = 10; // 蘇硉6ㄛ癹隅 WorkerId 郔湮硉峈2^6-1ㄛ撈蘇郔嗣盓厥64跺誹萸﹝
// options.SeqBitLength = 6; // 蘇硉6ㄛ癹秶藩瑭鏃汜傖腔ID跺杅﹝汜傖厒僅閉徹5勀跺/鏃ㄛ膘祜樓湮 SeqBitLength 善 10﹝
// options.BaseTime = Your_Base_Time; // 彆猁潭橾炵苀腔悕豪呾楊ㄛ森揭茼扢离峈橾炵苀腔BaseTime﹝
// ...... 坳統杅統蕉 IdGeneratorOptions 隅砱﹝

// 悵湔統杅ㄗ昢斛覃蚚ㄛ瘁寀統杅扢离祥汜虴ㄘㄩ
YitIdHelper.SetIdGenerator(options);

// 眕奻徹最硐剒擁珨棒ㄛ茼婓汜傖ID眳俇傖﹝



ProcessingDatabase DB = new(new()
{
    Server = "10.10.1.200",
    //Server = "127.0.0.1",
    User = "SLRLRES",
    Password = "SLRLRES",
    Database = "slrlres"
});

UserTable.User userTable = new(DB);
User_API.SetData(userTable);












var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();//鳳IP

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection().UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(_ => true));//賤樵輻郖恀枙

app.UseAuthorization();

app.MapControllers();

app.Run("http://0.0.0.0:5177");
