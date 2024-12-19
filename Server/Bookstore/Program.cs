using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAccessLayer.DataAccess;
using DataAccessLayer.Helpers;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System.Text;

// Configure and setup NLog
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Application is starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Ensure the "logs" folder exists within the project directory
    var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
    if (!Directory.Exists(logDirectory))
    {
        Directory.CreateDirectory(logDirectory);
    }

    // Add configuration to the DI container
    builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

    // Register DatabaseContext to DI container
    builder.Services.AddScoped<DatabaseContext>();

    // Setup NLog for Dependency Injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Register SqlHelper for Dependency Injection
    builder.Services.AddScoped<SqlHelper>();

    // Dependency Injection for Managers and Repositories
    builder.Services.AddTransient<IUserRepository, UserRepository>();
    builder.Services.AddTransient<IUserBL, UserBL>();
    builder.Services.AddTransient<TokenHelper>();

    // JWT Authentication configuration
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    })
    .AddJwtBearer("ResetPasswordScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:ResetPasswordKey"])),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });


    // Add controllers to the application
    builder.Services.AddControllers();

    // Configure Swagger/OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Bookstore",
            Version = "v1"
        });

        // Define the security scheme for Bearer tokens in Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter only the JWT token; 'Bearer' is added automatically."
        });

        // Add security requirement to enforce Bearer token use
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });
    // MassTransit configuration for RabbitMQ
    builder.Services.AddMassTransit(x =>
    {
        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
        {
            config.UseHealthCheck(provider);
            config.Host(new Uri("rabbitmq://localhost"), h =>
            {
                h.Username("guest");
                h.Password("guest");
            });
        }));
    });
    builder.Services.AddMassTransitHostedService();

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    

    // Use authentication and authorization middleware
    app.UseAuthentication();
    app.UseAuthorization();

    // Map controllers to endpoints
    app.MapControllers();

    // Run the application
    app.Run();
}
catch (Exception ex)
{
    // Log setup errors
    logger.Error(ex, "Application stopped because of an exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application exit
    LogManager.Shutdown();
}
