//using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PuppeteerSharp;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;
using System.Text;
using VietausWebAPI.Core.Application.Features.Labs.Helpers;
using VietausWebAPI.Core.Identity;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.Service;
using VietausWebAPI.Core.ServiceContracts;
using VietausWebAPI.Infrastructure.Repositories;
using VietausWebAPI.WebAPI;
using VietausWebAPI.WebAPI.DatabaseContext;
using VietausWebAPI.WebAPI.Hubs;


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseWebRoot("wwwroot");
//Add services to the container.

//builder.Services.AddControllers();

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder() // Yêu cầu tất cả controller phải gửi token mới được qua

    .RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Your API Name",
        Description = "Your API Description"
    });
});


//CORS: localhost: 4200, localhost: 4100
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder
        .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())
        .WithHeaders("Authorization", "origin", "accept", "content-type")
        .WithMethods("GET", "POST", "PUT", "DELETE")
        ;
    });

    options.AddPolicy("4100Client", policyBuilder =>
    {
        policyBuilder
        .WithOrigins(builder.Configuration.GetSection("AllowedOrigins2").Get<string[]>())
        .WithHeaders("Authorization", "origin", "accept")
        .WithMethods("GET")
        ;
    });

    options.AddPolicy("AllowAllOrigins",
    policy => policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
});

var fontPath = Path.Combine("wwwroot", "Fonts", "OpenSans-Regular.ttf");
FontManager.RegisterFont(File.OpenRead(fontPath));
QuestPDF.Settings.License = LicenseType.Community;


builder.Services.AddTransient<IJwtService, JwtService>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppDbConnectionString"));
});

builder.Services.AddApplicationServices();

builder.Services.AddSignalR();

// Thêm CORS để Blazor WebAssembly có thể kết nối
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://192.168.7.226:8081") // ⬅ đúng địa chỉ frontend
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


// Sử dụng AddIdentityCore khi không muốn sử dụng cookie vì identity tự động gọi sử dụng cookie
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

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<INotificationService, NotificationService>();


// Thử nghiệp phải xóa đi kkhi build trương trình
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false; // mặc định là false, chỉ để nhấn mạnh
});
// Thử nghiệp phải xóa đi kkhi build trương trình
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .Select(e => new
            {
                Field = e.Key,
                Errors = e.Value.Errors.Select(er => er.ErrorMessage).ToArray()
            });

        var json = System.Text.Json.JsonSerializer.Serialize(errors);
        Console.WriteLine("❌ Model binding errors: " + json);

        return new BadRequestObjectResult(context.ModelState);
    };
});
// Thử nghiệp phải xóa đi kkhi build trương trình
builder.Logging.ClearProviders();
// Thử nghiệp phải xóa đi kkhi build trương trình
builder.Logging.AddConsole();




//JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new
    TokenValidationParameters()
    {
        ValidateIssuer = true,
        //ValidIssuer = "https://mysso-server.com",
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidateAudience = true,
        //ValidAudience = "https://localhost:7264",
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(/*"your-25YOURYOUR_SUPER_SECRET_KEY_12345678901234567890"*/builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero

    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                //context.Response.Headers.Add("");
            }
            return Task.CompletedTask;
        },

    };
});

//builder.WebHost.UseUrls("http://192.168.7.226:8080");

var app = builder.Build();

app.UseCors("AllowAllOrigins");
//app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
});

//app.UseHttpsRedirection();

//app.UseRouting();

app.UseRouting(); // 🚨 BẮT BUỘC

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<NotificationHub>("/notificationHub");

app.MapControllers();

app.Run();