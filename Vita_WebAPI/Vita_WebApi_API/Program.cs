using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Serilog;
using Vita_WebApi_API.Dto;
using Vita_WebApi_API.MongoMapping;
using Vita_WebAPI_Repository;
using Vita_WebAPI_Services;
using Vita_WebAPI_Services.HealthCheck;

namespace Vita_WebApi_API;

public class Program
{
    public static Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        //Creates the builder 
        var builder = WebApplication.CreateBuilder(args);
        // Configure DbContext
        builder.Services.AddControllers(options => { options.SuppressAsyncSuffixInActionNames = false; });

        builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });


        // Configure MongoDB settings
        builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

        // Add MongoClient and MongoDatabase as singleton
        builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
        {
            var mongoDbSettings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(mongoDbSettings.ConnectionString);
        });

        builder.Services.AddSingleton<IMongoDatabase>(serviceProvider =>
        {
            var mongoDbSettings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            var client = serviceProvider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoDbSettings.DatabaseName);
        });

        // Register repositories and services
        builder.Services.AddScoped<IVideoService, VideoService>();
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IAuditLogService, AuditLogService>();

        builder.Services.AddRazorPages();
        
        //healthCheck
        builder.Services.AddHealthChecks()
            .AddCheck<HealthCheck>("HealthCheck")
            .AddCheck("MongoDbHealthCheck", new MongoDbHealthCheck(
                    $"mongodb://{builder.Configuration["MongoDbSettings:Host"]}:{builder.Configuration["MongoDbSettings:Port"]}",
                    builder.Configuration["MongoDbSettings:DatabaseName"]),
                tags: new[] { "db", "mongodb" });
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
        MongoDbClassMapping.RegisterClassMaps();
 
        // Configure Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "VITA API",
                Version = "v1",
                Description =
                    "The VITA API provides a set of endpoints for managing and interacting with video content within the VITA platform. This API allows users to perform operations such as retrieving video details, creating new video entries, and managing video metadata.",
                Contact = new OpenApiContact
                {
                    Name = "Andreas B",
                    Email = "Abarbesgaard@gmail.com"
                }
            });
            if (builder.Environment.IsProduction())
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    },
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insert token",
                };
                options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, Array.Empty<string>() }
                });
            }
            
        });
       

        // Build the app
        var app = builder.Build();
        app.Urls.Add("https://*:8080");
        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.InjectStylesheet("/css/swagger-ui/custom.css");
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "VITA API V1");
            });
            app.UseRouting();
            app.UseCors();
            app.MapControllers().AllowAnonymous(); // Allow anonymous access in development
            app.MapHealthChecks("/healthz", new HealthCheckOptions
                        {
                            ResponseWriter = HealthCheck.WriteResponse
                        });
           
        }
        else
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication(); // Enable authentication
            app.MapHealthChecks("/healthz", new HealthCheckOptions
            {
                ResponseWriter = HealthCheck.WriteResponse
            });
            app.MapControllers();   // Map controllers with authentication/authorization
        }

        app.Run();
        return Task.CompletedTask;
    }
}

public class MongoDbSettings
{
    public static string? Host { get; set; }
    /// <summary>
    /// The port of the database.
    /// </summary>
    public static int Port { get; set; }
    /// <summary>
    /// The name of the database.
    /// </summary>
    public string? DatabaseName { get; set; }
    /// <summary>
    /// The connection string for the database.
    /// </summary>
    public string ConnectionString => $"mongodb://{Host}:{Port}";
}

public class MongoDbRelease
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}