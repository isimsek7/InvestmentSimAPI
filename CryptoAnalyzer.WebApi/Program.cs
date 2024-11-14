using CryptoAnalyzer.Business.Settings;
using CryptoAnalyzer.Business.Operations.User;
using CryptoAnalyzer.Data.Context;
using CryptoAnalyzer.Data.Repositories;
using CryptoAnalyzer.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using CryptoAnalyzer.Business.DataProtection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CryptoAnalyzer.WebApi.Middlewares;
using CryptoAnalyzer.Business.Operations.Investments;
using CryptoAnalyzer.Business.Operations.Portfolios;
using CryptoAnalyzer.Business.Operations.TransactionHistories;
using System.Text.Json.Serialization;
using CryptoAnalyzer.Business.InvestmentGroups;
using CryptoAnalyzer.Business.UserInvestmentGroups;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Investment Simulator API",
        Version = "v1"
    });

    // Define the JWT Bearer authentication scheme in Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insert the token"
    });

    // Set up the security requirements
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<IDataProtection, DataProtection>();

var keysDirectory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys"));

builder.Services.AddDataProtection()
    .SetApplicationName("CryptoAnalyzer")
    .PersistKeysToFileSystem(keysDirectory);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
        };

        // Optional: Configure error handling for JWT
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.NoResult(); // Suppresses default authentication error response
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("Unauthorized: Invalid token or expired session");
            }
        };
    });

builder.Services.AddDbContext<CryptoAnalyzerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ISettingService, SettingManager>();
builder.Services.AddScoped<IInvestmentService, InvestmentService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<ITransactionHistoryService,TransactionHistoryManager>();
builder.Services.AddScoped<IInvestmentGroupService, InvestmentGroupManager>();
builder.Services.AddScoped<IUserInvestmentGroupsService, UserInvestmentGroupsManager>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMaintenanceMode();

app.UseGlobalException();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

