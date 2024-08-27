using System.Reflection;
using ClassLibrary1Vita_WebAPI_Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Serilog;
using Vita_WebAPI_Data;
using Vita_WebAPI_Repository;
using Vita_WebAPI_Services;

namespace Vita_WebApi_API;
public class Program
{
    public static void Main(string[] args)
    {
        
        var builder = WebApplication.CreateBuilder(args);
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        builder.Services.AddScoped<IVideoRepository, VideoRepository>();
        builder.Services.AddScoped<IVideoService, VideoService>();
        builder.Services.AddDbContext<DataContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
        builder.Services.AddControllersWithViews();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen( options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "VITA API", 
                Version = "v1", 
                Description = "The VITA API provides a set of endpoints for managing and interacting with video content within the VITA platform. This API allows users to perform operations such as retrieving video details, creating new video entries, and managing video metadata.",
                Contact = new OpenApiContact
                {
                    Name = "Andreas B",
                    Email = "Abarbesgaard@gmail.com"
                }   
            });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename)); 
                    
        });
        
        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
            {
                options.InjectStylesheet("/css/swagger-ui/custom.css");
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "VITA API V1");
            });
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
     

        app.MapControllers();

        app.Run();
    }
}
