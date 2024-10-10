using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Vita_WebAPI_Services.HealthCheck;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Vita_Test;

[TestClass]
public class MongoDbHealthCheckTests
{
    private const string ValidConnectionString = "mongodb://localhost:27017";
    private const string ValidDatabaseName = "Catalog";

    private MongoDbHealthCheck _healthCheck = null!;

    [TestInitialize]
    public void Setup()
    {
        _healthCheck = new MongoDbHealthCheck(ValidConnectionString, ValidDatabaseName);
    }

    [TestMethod]
    public async Task CheckHealthAsync_ShouldReturnHealthy_WhenMongoDbIsReachable()
    {
        // Act
        var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        result.Status.Should().Be(HealthStatus.Healthy);
        result.Description.Should().Be("MongoDB is healthy");
    }

    [TestMethod]
    public async Task CheckHealthAsync_ShouldReturnUnhealthy_WhenMongoDbIsNotReachable()
    {
        // Arrange
        var invalidConnectionString = "mongodb://invalidhost:27017";
        var healthCheck = new MongoDbHealthCheck(invalidConnectionString, ValidDatabaseName);

        // Act
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
        result.Description.Should().Be("MongoDB is unhealthy");
        result.Data["ErrorMessage"].Should().NotBeNull();
    }
}

[TestClass]
public class HealthCheckTests
{
    private HealthCheck? _healthCheck;

    [TestInitialize]
    public void Setup()
    {
        _healthCheck = new HealthCheck(); // For default sunde tilstand
    }

    [TestMethod]
    public async Task CheckHealthAsync_ShouldReturnHealthy_WhenConditionIsTrue()
    {
        // Act
        if (_healthCheck != null)
        {
            var result = await _healthCheck.CheckHealthAsync(
                new HealthCheckContext(),
                CancellationToken.None
            );

            // Assert
            result.Status.Should().Be(HealthStatus.Healthy);
            result.Description.Should().Be("Healthy API");
        }
    }

    [TestMethod]
    public async Task CheckHealthAsync_ShouldReturnUnhealthy_WhenConditionIsFalse()
    {
        // Arrange
        var healthCheck = new HealthCheck(isHealthy: false); // Konfigurér til at være usund

        // Act
        var result = await healthCheck.CheckHealthAsync(
            new HealthCheckContext(),
            CancellationToken.None
        );

        // Assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
        result.Description.Should().Be("Unhealthy API");
    }

    [TestMethod]
    public async Task WriteResponse_ShouldWriteExpectedJson()
    {
        // Arrange
        var healthReport = new HealthReport(
            new Dictionary<string, HealthReportEntry>
            {
                {
                    "check1",
                    new HealthReportEntry(
                        HealthStatus.Healthy,
                        "All good",
                        TimeSpan.Zero,
                        null,
                        null
                    )
                },
                {
                    "check2",
                    new HealthReportEntry(
                        HealthStatus.Unhealthy,
                        "Something went wrong",
                        TimeSpan.Zero,
                        null,
                        null
                    )
                }
            },
            TimeSpan.Zero
        );

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream(); // Ensure the response body is reset

        // Act
        await HealthCheck.WriteResponse(context, healthReport);

        // Read the response
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();

        // Assert
        var expectedJson = new
        {
            status = "Unhealthy",
            results = new
            {
                check1 = new
                {
                    status = "Healthy",
                    description = "All good",
                    data = new { }
                },
                check2 = new
                {
                    status = "Unhealthy",
                    description = "Something went wrong",
                    data = new { }
                }
            }
        };

        var actualJson = JsonSerializer.Deserialize<JsonElement>(responseBody);
        Assert.AreEqual(expectedJson.status, actualJson.GetProperty("status").GetString());
        Assert.AreEqual(
            expectedJson.results.check1.status,
            actualJson
                .GetProperty("results")
                .GetProperty("check1")
                .GetProperty("status")
                .GetString()
        );
        Assert.AreEqual(
            expectedJson.results.check1.description,
            actualJson
                .GetProperty("results")
                .GetProperty("check1")
                .GetProperty("description")
                .GetString()
        );
        Assert.AreEqual(
            expectedJson.results.check2.status,
            actualJson
                .GetProperty("results")
                .GetProperty("check2")
                .GetProperty("status")
                .GetString()
        );
        Assert.AreEqual(
            expectedJson.results.check2.description,
            actualJson
                .GetProperty("results")
                .GetProperty("check2")
                .GetProperty("description")
                .GetString()
        );
    }
}

