
using Microsoft.EntityFrameworkCore;

using NewGaras.Domain.Mappers;
using NewGaras.Domain.Services;
using NewGaras.Domain.Services.TransportationLineService;
using NewGaras.Infrastructure;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Helper.TenantService;

using NewGaras.Infrastructure.Interfaces.ServicesInterfaces.TransportationLine;
using NewGaras.Infrastructure.Models.Mail;
using NewGarasAPI.Hubs;
using NewGarasAPI.Models;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {

            //you can configure your custom policy
            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});
// Add services to the container.
//builder.Services.AddDbContext<GarasTestContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("GarasTest"))
//);
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ITenantService, TenantService>();
builder.Services.Configure<TenantSettings>(builder.Configuration.GetSection(nameof(TenantSettings)));
TenantSettings options = new();
builder.Configuration.GetSection(nameof(TenantSettings)).Bind(options);

var defaultDbProvider = options.Defaults.DBProvider;

//builder.Services.AddDbContext<GarasTestContext>((serviceProvider, options) =>
//{
//    var tenantResolver = serviceProvider.GetRequiredService<ITenantService>();
//    var tenant = tenantResolver.GetTenant();
//    options.UseSqlServer(tenant.ConnectionString, options =>
//    {
//        options.CommandTimeout(360); // seconds (e.g. 6 minutes)
//    });
//});

//builder.Services.AddDbContext<PharmaTransportationContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("PharmaTransportation")));
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);


builder.Services.AddDbContext<GarasTestContext>();


//____________________________________________ TransportationLine ____________________________________________
builder.Services.AddScoped<ITransportationLineService, TransportationLineService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IHrUserService, HrUserService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

builder.Services.AddScoped<IMailService, MailService>();

//-----------------------------------------------Medical------------------------------


//-----------------------------------------------Library------------------------------



//provider =>
//   new GraphAuthService(
//       builder.Configuration["AzureAd:ClientId"],
//       builder.Configuration["AzureAd:TenantId"],
//       builder.Configuration["AzureAd:ClientSecret"]
//   ));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddSignalR().AddHubOptions<NotificationsHub>(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromMinutes(30);
    options.KeepAliveInterval = TimeSpan.FromMinutes(15);
    //options.Hubs.EnableDetailedErrors = true;
});


//builder.Services.AddQuartz(q =>
//{
//    // إنشاء مفتاح فريد للـ Job
//    var jobKey = new JobKey("AttendanceJob");

//    // تعريف الـ Job
//    q.AddJob<AttendanceJob>(opts => opts.WithIdentity(jobKey));

//    // إنشاء التوقيت (كل 12 ساعة)
//    q.AddTrigger(opts => opts
//        .ForJob(jobKey)
//        .WithIdentity("AttendanceJob-trigger")
//        // استخدام Cron Expression لضبط الوقت بدقة (أو Simple Schedule)
//        .WithSimpleSchedule(x => x
//            .WithIntervalInMinutes(3)
//            .RepeatForever())
//    );
//});


// Get the ServerName from AppSettings
var appSettings = builder.Configuration.GetSection("AppSettings");
var serverName = appSettings["ServerName"] ?? "";
var garasConnectionString = builder.Configuration.GetConnectionString("GarasTest");

// Only configure Quartz if ServerName is NOT "GARASSOLUTIONS"
if(serverName.Equals("PCOMPLAINS" , StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddQuartz(q =>
    {
        var garasConnectionString = builder.Configuration.GetConnectionString("GarasTest");

        var jobKey = new JobKey("AttendanceJob");

        q.AddJob<AttendanceJob>(opts => opts
            .WithIdentity(jobKey)
            .UsingJobData("GARASTransportation" , garasConnectionString)
        );

        q.AddTrigger(opts => opts
         .ForJob(jobKey)
         .WithIdentity("AttendanceJob-trigger")
         .StartAt(DateTimeOffset.Now.AddMinutes(1))
         .WithSimpleSchedule(x => x
             .WithIntervalInHours(12)
             .RepeatForever()
         )
     );
    });

    builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete=true);
}



var app = builder.Build();
app.UseCors();





//  For Periti Only
// Configure CORS policy to allow a specific URL (e.g., Flutter app)
//var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

////builder.Services.AddCors(options =>
////{
////    options.AddPolicy(name: myAllowSpecificOrigins,
////        builder =>
////        {
////            // Allow the Flutter app URL
////            builder.WithOrigins("http://192.168.1.68:8080")
////                   .AllowAnyHeader()
////                   .AllowAnyMethod()
////                   .AllowCredentials();  // Optional if you need cookies or credentials
////        });
////});
//// Enable the configured CORS policy globally
//app.UseCors(myAllowSpecificOrigins);
//var app = builder.Build();









//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.MapHub<NotificationsHub>("/Notifications");


app.Run();





