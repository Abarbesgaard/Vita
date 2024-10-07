using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Vita_WebApi_Shared;

namespace Vita_Test;

[TestClass]
public class ModelTests
{
    [TestCategory("Video")]
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

    [TestCategory("Video")]
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

    [TestCategory("Video")]
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

    [TestCategory("Video")]
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

    [TestCategory("Video")]
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
    [TestCategory("BaseEntity")]
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

    [TestCategory("Activity")]
    [TestMethod]
    public void Activity_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var createdAt = DateTimeOffset.Now;
        var updatedAt = DateTimeOffset.Now.AddHours(1);
        var hostId = Guid.NewGuid(); // Generate a new HostId
        var startTime = DateTimeOffset.Now.AddHours(2); // Start time in the future
        var endTime = startTime.AddHours(1); // End time 1 hour after start

        // Act
        var activity = new Activity
        {
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            Title = "Sample Title",
            Description = "Sample Description",
            HostId = hostId,
            Start = startTime,
            End = endTime,
            Attendee = new List<Users>(), // Initialize with an empty list
            VerifiedAttendee = new List<Users>(),
            DeclinedAttendee = new List<Users>(),
            TentativeAttendee = new List<Users>()
        };

        // Assert
        activity.CreatedAt.Should().Be(createdAt);
        activity.UpdatedAt.Should().Be(updatedAt);
        activity.Title.Should().Be("Sample Title");
        activity.Description.Should().Be("Sample Description");
        activity.HostId.Should().Be(hostId); // Assert HostId
        activity.Start.Should().Be(startTime); // Assert Start
        activity.End.Should().Be(endTime); // Assert End
        activity.Attendee.Should().NotBeNull().And.BeEmpty(); // Assert Attendees are initialized
        activity.VerifiedAttendee.Should().NotBeNull().And.BeEmpty(); // Assert Verified Attendees
        activity.DeclinedAttendee.Should().NotBeNull().And.BeEmpty(); // Assert Declined Attendees
        activity.TentativeAttendee.Should().NotBeNull().And.BeEmpty(); // Assert Tentative Attendees
    }
}