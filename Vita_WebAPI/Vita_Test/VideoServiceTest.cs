using FluentAssertions;
using Vita_WebAPI_Services;

namespace Vita_Test;

[TestClass]
public class VideoServiceTest
{
    [TestMethod]
    public void JsonStringBuilder_ShouldReturnString()
    {
        
        //Act
        var result = Auth0Service.JsonStringBuilder();

        //Assert
        result.Should().NotBeNull("Because the JSON string should be valid");
        result.Should().BeOfType<string>("Because the result should be a string");
        result.Should().StartWith("{").And.EndWith("}");
    }
}