

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
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
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
//builder.Services.AddCors(o =>
//{
//    o.AddPolicy("Frontend", p => p
//        .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
//        .AllowAnyHeader()
//        .AllowAnyMethod()
//        .AllowCredentials());
//});

//// Services
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFrontend", p => p
//        .WithOrigins(
//            "http://localhost:5076",   // FE http
//            "https://localhost:5076",   // FE https (nếu FE chạy https)
//            "http://192.168.7.4:8086"
//        )
//        .AllowAnyHeader()
//        .AllowAnyMethod()
//        .AllowCredentials()           // SignalR cần
//    );
//});

// Program.cs (services)
builder.Services.AddCors(o =>
{
    o.AddPolicy("DevAll", p => p
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
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
builder.Services.AddScoped<IVisibilityHelper, VisibilityHelper>();
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


// Program.cs (pipeline) – đặt trước auth/map
app.UseCors("DevAll");


// ===== Pipeline =====
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

// CORS phải trước Auth & trước MapHub
//app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

// Map Controllers trước hoặc sau MapHub đều được, miễn là sau UseRouting
app.MapControllers();

// Map đúng MỘT hub (đường dẫn phải khớp client)
app.MapHub<NotificationHub>("/hubs/notifications");

// Nếu host SPA cùng server thì mới cần fallback (đặt sau MapHub)
// app.MapFallbackToFile("index.html");

app.Run();
