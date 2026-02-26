using Unit_03_RSVP_App.Models;
using Microsoft.AspNetCore.Mvc;
using Unit_03_RSVP_App.Controllers;
using Xunit;
using System.ComponentModel.DataAnnotations;

namespace Unit_03_RSVP_App.Tests;

public class RsvpTests
{
    [Fact]
    public void GuestResponse_Validation_Works()
    {
        // Arrange
        var response = new GuestResponse { Name = "Test", Email = "invalid", Phone = "123", WillAttend = true };
        var context = new ValidationContext(response, null, null);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(response, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Email"));
    }

    [Fact]
    public void HomeController_Post_RsvpForm_Valid_Redirects()
    {
        // Arrange
        var controller = new HomeController();
        var response = new GuestResponse { Name = "Test", Email = "test@example.com", Phone = "123", WillAttend = true };

        // Act
        var result = controller.RsvpForm(response);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Thanks", viewResult.ViewName);
        Assert.Equal(response, viewResult.Model);
    }
}
