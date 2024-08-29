using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Serilog;
using Vita_WebAPI_Data;
using Vita_WebAPI_Repository;
using Vita_WebAPI_Services;

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

        builder.Services.Configure<VideoDatabaseSetting>(builder.Configuration.GetSection(nameof(MongoDbSettings)));

        builder.Services.AddSingleton(serviceProvider =>
        {
            var videoDatabaseSetting = serviceProvider.GetRequiredService<IOptions<VideoDatabaseSetting>>().Value;
            var mongoClient = new MongoClient(videoDatabaseSetting.ConnectionString);
            return mongoClient.GetDatabase(videoDatabaseSetting.DatabaseName);
        });

        builder.Services.AddScoped<IVideoRepository, VideoRepository>();
        builder.Services.AddScoped<IVideoService, VideoService>();

        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        builder.Services.AddAuthentication().AddJwtBearer();
        builder.Services.AddAuthentication();
        
        
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
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
            
        });
       

        // Build the app
        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.InjectStylesheet("/css/swagger-ui/custom.css");
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "VITA API V1");
            });
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
        return Task.CompletedTask;
    }
}

public class MongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}