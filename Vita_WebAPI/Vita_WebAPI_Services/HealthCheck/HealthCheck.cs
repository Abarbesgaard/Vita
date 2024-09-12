using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Vita_WebAPI_Services.HealthCheck;
public class MongoDbHealthCheck : IHealthCheck
{
    private readonly string _connectionString;
    private readonly string? _databaseName;

    public MongoDbHealthCheck(string connectionString, string? databaseName)
    {
        _connectionString = connectionString;
        _databaseName = databaseName;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(_databaseName);
            await database.RunCommandAsync((Command<BsonDocument>)"{ping:1}", cancellationToken: cancellationToken);
            return HealthCheckResult.Healthy("MongoDB is healthy");
        }
        catch (Exception ex)
        {
            // Returnér fejlbesked sammen med undtagelsen for at få flere detaljer om fejlen
            return HealthCheckResult.Unhealthy("MongoDB is unhealthy", ex, new Dictionary<string, object>
            {
                { "ErrorMessage", ex.Message }
            });
        }
    }
}

public class HealthCheck : IHealthCheck
{
    private readonly bool _isHealthy;

    public HealthCheck(bool isHealthy = true)
    {
        _isHealthy = isHealthy;
    } 
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        if (_isHealthy)
        {
            return Task.FromResult(HealthCheckResult.Healthy("Healthy API"));
        }
    
        var failureStatus = context?.Registration?.FailureStatus ?? HealthStatus.Unhealthy;
        return Task.FromResult(new HealthCheckResult(
            failureStatus, "Unhealthy API"));
    }
    

    public static Task WriteResponse(HttpContext context, HealthReport healthReport)
{
    context.Response.ContentType = "application/json; charset=utf-8";

    var options = new JsonWriterOptions { Indented = true };

    using var memoryStream = new MemoryStream();
    using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
    {
        jsonWriter.WriteStartObject();
        jsonWriter.WriteString("status", healthReport.Status.ToString()); // Make sure this matches your expected status
        jsonWriter.WriteStartObject("results");

        foreach (var healthReportEntry in healthReport.Entries)
        {
            jsonWriter.WriteStartObject(healthReportEntry.Key);
            jsonWriter.WriteString("status", healthReportEntry.Value.Status.ToString());
            jsonWriter.WriteString("description", healthReportEntry.Value.Description);
            jsonWriter.WriteStartObject("data");

            foreach (var item in healthReportEntry.Value.Data)
            {
                jsonWriter.WritePropertyName(item.Key);
                JsonSerializer.Serialize(jsonWriter, item.Value, item.Value.GetType());
            }

            jsonWriter.WriteEndObject();
            jsonWriter.WriteEndObject();
        }

        jsonWriter.WriteEndObject();
        jsonWriter.WriteEndObject();
    }

    return context.Response.WriteAsync(Encoding.UTF8.GetString(memoryStream.ToArray()));
}

    
}