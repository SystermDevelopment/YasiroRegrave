using Serilog;
using Microsoft.EntityFrameworkCore;
using YasiroRegrave.Data;
using YasiroRegrave.Middleware;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(
           builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddRazorPages();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/regrave_.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// サービスにコントローラーを追加
builder.Services.AddControllers();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // セッションの有効期限
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // Kestrel サーバーの設定を追加
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(5006, listenOptions =>
        {
            listenOptions.UseHttps(); // 必要に応じてHTTPSを設定
        });
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.Urls.Add("https://localhost:5001");
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

if (!app.Environment.IsDevelopment())
{
    app.MapControllers().RequireHost("*:5006");
}

app.Run();
