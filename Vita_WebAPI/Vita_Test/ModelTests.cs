using FluentAssertions;
using Vita_WebAPI_Data;

namespace Vita_Test;

[TestClass]
public class ModelTests
{
     [TestMethod]
        public void ConnectionString_ShouldReturnCorrectFormat()
        {
            // Arrange
            var host = "localhost";
            var port = 27017;
            var expectedConnectionString = $"mongodb://{host}:{port}";
    
            var settings = new VideoDatabaseSetting
            {
                Host = host,
                Port = port
            };
    
            // Act
            var connectionString = settings.ConnectionString;
    
            // Assert
            connectionString.Should().BeEquivalentTo(expectedConnectionString);
        }
}