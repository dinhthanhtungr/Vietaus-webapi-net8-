////using Asp.Versioning;
//using AutoMapper; // Add this at the top of the file
//using AutoMapper.Execution;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http.Features;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Authorization;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.IdentityModel.Tokens;
//using PuppeteerSharp;
//using QuestPDF.Drawing;
//using QuestPDF.Infrastructure;
//using System;
//using System.Text;
//using VietausWebAPI.Core.Application.Features.HR;
//using VietausWebAPI.Core.Application.Features.Labs;
//using VietausWebAPI.Core.Application.Features.Manufacturing;
//using VietausWebAPI.Core.Application.Features.MaterialFeatures;
//using VietausWebAPI.Core.Application.Features.Planning;
//using VietausWebAPI.Core.Application.Features.Sales;
//using VietausWebAPI.Core.Application.Shared.Helper.FileStorage;
//using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
//using VietausWebAPI.Core.Application.Shared.Helper.Repository;
//using VietausWebAPI.Core.Identity;
//using VietausWebAPI.Core.Repositories_Contracts;
//using VietausWebAPI.Infrastructure.Helpers.Repositories;
//using VietausWebAPI.Infrastructure.Repositories;
//using VietausWebAPI.WebAPI;
//using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;
//using VietausWebAPI.WebAPI.Helpers;
//using VietausWebAPI.WebAPI.Hubs;
//using VietausWebAPI.WebAPI.DependencyInjections;
//using VietausWebAPI.Core.Application.Features.Shared.ServiceContracts;
//using VietausWebAPI.Core.Application.Features.Shared.Service;


//var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.UseWebRoot("wwwroot");
////Add services to the container.

////builder.Services.AddControllers();

//// Program.cs (đặt ở NHỮNG DÒNG ĐẦU TIÊN)
////AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//builder.Services.AddControllers(options =>
//{
//    var policy = new AuthorizationPolicyBuilder() // Yêu cầu tất cả controller phải gửi token mới được qua

//    .RequireAuthenticatedUser().Build();
//    options.Filters.Add(new AuthorizeFilter(policy));
//}).AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
//});





//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
//    {
//        Version = "v1",
//        Title = "Your API Name",
//        Description = "Your API Description"
//    });
//});


////CORS: localhost: 4200, localhost: 4100
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(policyBuilder =>
//    {
//        policyBuilder
//        .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())
//        .WithHeaders("Authorization", "origin", "accept", "content-type")
//        .WithMethods("GET", "POST", "PUT", "DELETE")
//        ;
//    });

//    options.AddPolicy("4100Client", policyBuilder =>
//    {
//        policyBuilder
//        .WithOrigins(builder.Configuration.GetSection("AllowedOrigins2").Get<string[]>())
//        .WithHeaders("Authorization", "origin", "accept")
//        .WithMethods("GET")
//        ;
//    });

//    options.AddPolicy("AllowAllOrigins",
//    policy => policy.AllowAnyOrigin()
//                    .AllowAnyMethod()
//                    .AllowAnyHeader());
//});

//var fontPath = Path.Combine("wwwroot", "Fonts", "OpenSans-Regular.ttf");
//FontManager.RegisterFont(File.OpenRead(fontPath));
//QuestPDF.Settings.License = LicenseType.Community;


//builder.Services.AddTransient<IJwtService, JwtService>();



//builder.Services.AddAutoMapper(
//    cfg => { },
//    typeof(HRMappingProfile).Assembly,   // Infrastructure/Application profile assembly
//    typeof(PlanningMappingProfile).Assembly,
//    typeof(ProductStandardMappingProfile).Assembly,
//    typeof(SaleMappingProfile).Assembly,
//    typeof(MaterialMappingProfile).Assembly,
//    typeof(ManufacturingMappingProfile).Assembly

//);

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseNpgsql(builder.Configuration.GetConnectionString("AppDbConnectionString"));
//});

//builder.Services.AddApplicationServices();

//builder.Services.AddSignalR();

//// Thêm CORS để Blazor WebAssembly có thể kết nối
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFrontend", policy =>
//    {
//        policy.WithOrigins("http://192.168.7.226:8081") // ⬅ đúng địa chỉ frontend
//              .AllowAnyHeader()
//              .AllowAnyMethod()
//              .AllowCredentials();
//    });
//});


//// Sử dụng AddIdentityCore khi không muốn sử dụng cookie vì identity tự động gọi sử dụng cookie
//builder.Services.AddIdentityCore<ApplicationUser>(options =>
//{
//    options.Password.RequiredLength = 5; 
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireDigit = false;
//    options.Password.RequireLowercase = true;
//    options.Password.RequireUppercase = false;
//})
//.AddRoles<ApplicationRole>()
//.AddEntityFrameworkStores<ApplicationDbContext>()
//.AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
//.AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>()
//.AddSignInManager()
//.AddDefaultTokenProviders();



//builder.Services.AddHttpContextAccessor();

//builder.Services.AddScoped<ICurrentUser, CurrentUser>();
//builder.Services.AddScoped<IJwtService, JwtService>();
//builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


//// Thử nghiệp phải xóa đi kkhi build trương trình
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = false; // mặc định là false, chỉ để nhấn mạnh
//});
//// Thử nghiệp phải xóa đi kkhi build trương trình
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.InvalidModelStateResponseFactory = context =>
//    {
//        var errors = context.ModelState
//            .Where(e => e.Value.Errors.Count > 0)
//            .Select(e => new
//            {
//                Field = e.Key,
//                Errors = e.Value.Errors.Select(er => er.ErrorMessage).ToArray()
//            });

//        var json = System.Text.Json.JsonSerializer.Serialize(errors);
//        Console.WriteLine("❌ Model binding errors: " + json);

//        return new BadRequestObjectResult(context.ModelState);
//    };
//});

//// File storage
//builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection("Storage"));
//builder.Services.AddSingleton<IFileShareStorage, FileShareStorage>();

//// Cho upload lớn
//builder.Services.Configure<FormOptions>(o => o.MultipartBodyLengthLimit = 50_000_000);




//// Thử nghiệp phải xóa đi kkhi build trương trình
//builder.Logging.ClearProviders();
//// Thử nghiệp phải xóa đi kkhi build trương trình
//builder.Logging.AddConsole();


//// 🔽 Ở đây, khi đăng ký DbContext
//builder.Services.AddDbContext<ApplicationDbContext>(opt => opt
//    .UseNpgsql(builder.Configuration.GetConnectionString("AppDbConnectionString"))
//    .EnableDetailedErrors()
//    .EnableSensitiveDataLogging()
//    .LogTo(Console.WriteLine, LogLevel.Information));


////JWT
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})


////builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new
//    TokenValidationParameters()
//    {
//        ValidateIssuer = true,
//        //ValidIssuer = "https://mysso-server.com",
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],

//        ValidateAudience = true,
//        //ValidAudience = "https://localhost:7264",
//        ValidAudience = builder.Configuration["Jwt:Audience"],

//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey
//        (Encoding.UTF8.GetBytes(/*"your-25YOURYOUR_SUPER_SECRET_KEY_12345678901234567890"*/builder.Configuration["Jwt:Key"])),
//        ClockSkew = TimeSpan.Zero

//    };

//    options.Events = new JwtBearerEvents
//    {
//        OnAuthenticationFailed = context =>
//        {
//            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
//            {
//                //context.Response.Headers.Add("");
//            }
//            return Task.CompletedTask;
//        },

//    };
//});


//builder.Services.AddSignalR();


////builder.WebHost.UseUrls("http://192.168.7.226:8080");

//var app = builder.Build();


////app.UseCors("AllowFrontend");

//// Configure the HTTP request pipeline.

//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
//});
//app.MapHub<NotificationHub>("/hubs/notifications");

////app.UseHttpsRedirection();

////app.UseRouting();

//app.UseRouting(); // 🚨 BẮT BUỘC
//app.UseCors("AllowAllOrigins");
//app.UseAuthentication();

//app.UseAuthorization();

//app.MapHub<NotificationHub>("/notificationHub");

//app.MapControllers();

//app.Run();











using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using System.Text;
using VietausWebAPI.Core.Application.Features.HR;
using VietausWebAPI.Core.Application.Features.Labs;
using VietausWebAPI.Core.Application.Features.Manufacturing;
using VietausWebAPI.Core.Application.Features.MaterialFeatures;
using VietausWebAPI.Core.Application.Features.Planning;
using VietausWebAPI.Core.Application.Features.Sales;
using VietausWebAPI.Core.Application.Features.Shared.Service;
using VietausWebAPI.Core.Application.Features.Shared.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper.FileStorage;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Identity;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;
using VietausWebAPI.Infrastructure.Helpers.Repositories;
using VietausWebAPI.WebAPI.DependencyInjections; // nơi có AddApplicationServices() theo module
using VietausWebAPI.WebAPI.Helpers;
using VietausWebAPI.WebAPI.Hubs;
// Nếu bạn dùng OutboxProcessor:
// using VietausWebAPI.WebAPI.Background;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseWebRoot("wwwroot");

// ===== Controllers (yêu cầu JWT cho tất cả API) =====
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
})
.AddJsonOptions(o => o.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

// ===== Swagger =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Your API Name",
        Description = "Your API Description"
    });
});

// ===== CORS (duy nhất 1 policy cho FE) =====
// Đặt các origin FE thật của bạn trong appsettings.json:
// "AllowedOrigins": ["http://192.168.7.226:8081","https://localhost:5173","https://localhost:7164"]
builder.Services.AddCors(o =>
{
    o.AddPolicy("Frontend", p => p
        .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

// Services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", p => p
        .WithOrigins(
            "http://localhost:5076",   // FE http
            "https://localhost:5076"   // FE https (nếu FE chạy https)
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()           // SignalR cần
    );
});


// ===== QuestPDF & Font =====
var fontPath = Path.Combine("wwwroot", "Fonts", "OpenSans-Regular.ttf");
QuestPDF.Drawing.FontManager.RegisterFont(File.OpenRead(fontPath));
QuestPDF.Settings.License = LicenseType.Community;

// ===== AutoMapper (giữ đúng các profile bạn đã đăng ký) =====
builder.Services.AddAutoMapper(
    cfg => { },
    typeof(HRMappingProfile).Assembly,
    typeof(PlanningMappingProfile).Assembly,
    //typeof(ProductStandardMappingProfile).Assembly,
    typeof(SaleMappingProfile).Assembly,
    typeof(MaterialMappingProfile).Assembly,
    typeof(ManufacturingMappingProfile).Assembly
);

// ===== DbContext (đăng ký MỘT LẦN) =====
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt
    .UseNpgsql(builder.Configuration.GetConnectionString("AppDbConnectionString"))
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, LogLevel.Information));

// ===== Identity + Roles (không dùng cookie) =====
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
})
.AddRoles<ApplicationRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
.AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>()
.AddSignInManager()
.AddDefaultTokenProviders();

// ===== JWT (thêm OnMessageReceived cho SignalR) =====
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ClockSkew = TimeSpan.Zero
        };

        // Cho phép SignalR lấy token qua query khi upgrade WebSocket
        o.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                var path = ctx.HttpContext.Request.Path;
                if (path.StartsWithSegments("/hubs/notifications"))
                {
                    var accessToken = ctx.Request.Query["access_token"];
                    if (!string.IsNullOrEmpty(accessToken))
                        ctx.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// ===== Các services phụ trợ =====
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// File storage
builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection("Storage"));
builder.Services.AddSingleton<IFileShareStorage, FileShareStorage>();

// Upload lớn
builder.Services.Configure<FormOptions>(o => o.MultipartBodyLengthLimit = 50_000_000);

// Logging dev
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// ===== SignalR =====
builder.Services.AddSignalR();

// ===== Đăng ký DI theo module + UoW =====
builder.Services.AddApplicationServices(); // RootApplicationDI: AddAttachmentsModule(), AddNotificationsModule(), ...

// Nếu dùng OutboxProcessor để đẩy notify realtime từ Outbox, bật dòng dưới:
// builder.Services.AddHostedService<OutboxProcessor>();

var app = builder.Build();

// ===== Pipeline =====
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

// CORS phải trước Auth & trước MapHub
app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

// Map Controllers trước hoặc sau MapHub đều được, miễn là sau UseRouting
app.MapControllers();

// Map đúng MỘT hub (đường dẫn phải khớp client)
app.MapHub<NotificationHub>("/hubs/notifications");

// Nếu host SPA cùng server thì mới cần fallback (đặt sau MapHub)
// app.MapFallbackToFile("index.html");

app.Run();
