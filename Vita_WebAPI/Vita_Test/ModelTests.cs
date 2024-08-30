using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Vita_WebApi_Shared;

namespace Vita_Test;

[TestClass]
public class ModelTests
{
    [TestMethod]
    public void Video_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var createdAt = DateTimeOffset.Now;
        var updatedAt = DateTimeOffset.Now.AddHours(1);

        // Act
        var video = new Video
        {
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            Title = "Sample Title",
            Description = "Sample Description",
            Url = "https://example.com/video"
        };

        // Assert
        video.CreatedAt.Should().Be(createdAt);
        video.UpdatedAt.Should().Be(updatedAt);
        video.Title.Should().Be("Sample Title");
        video.Description.Should().Be("Sample Description");
        video.Url.Should().Be("https://example.com/video");
    }

    [TestMethod]
    public void Video_ShouldFailValidation_WhenTitleIsTooLong()
    {
        // Arrange
        var video = new Video
        {
            Title = new string('a', 201), // Title length > 200
            Description = "Valid Description",
            Url = "https://example.com/video"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(video);
        var isValid = Validator.TryValidateObject(video, validationContext, validationResults, true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("Title must be between 1 and 200 characters");
    }

    [TestMethod]
    public void Video_ShouldFailValidation_WhenDescriptionIsTooLong()
    {
        // Arrange
        var video = new Video
        {
            Title = "Valid Title",
            Description = new string('a', 501), // Description length > 500
            Url = "https://example.com/video"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(video);
        var isValid = Validator.TryValidateObject(video, validationContext, validationResults, true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("Description must be between 1 and 500 characters");
    }

    [TestMethod]
    public void Video_ShouldFailValidation_WhenUrlIsTooLong()
    {
        // Arrange
        var video = new Video
        {
            Title = "Valid Title",
            Description = "Valid Description",
            Url = new string('a', 2084) // Url length > 2083
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(video);
        var isValid = Validator.TryValidateObject(video, validationContext, validationResults, true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("Url must be between 1 and 2083 characters");
    }

    [TestMethod]
    public void Video_ShouldSucceedValidation_WhenPropertiesAreValid()
    {
        // Arrange
        var video = new Video
        {
            Title = "Valid Title",
            Description = "Valid Description",
            Url = "https://example.com/video"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(video);
        var isValid = Validator.TryValidateObject(video, validationContext, validationResults, true);

        // Assert
        isValid.Should().BeTrue();
        validationResults.Should().BeEmpty();
    }
    [TestMethod]
    public void BaseEntity_Id_ShouldBeInitializedCorrectly()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var entity = new Video { Id = expectedId, Url = ""};

        // Act
        var actualId = entity.Id;

        // Assert
        actualId.Should().Be(expectedId);
    }
}