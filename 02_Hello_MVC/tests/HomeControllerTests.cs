using Microsoft.AspNetCore.Mvc;
using Unit_02_Hello_MVC.Controllers;
using Unit_02_Hello_MVC.Models;
using Xunit;

namespace Unit_02_Hello_MVC.Tests;

public class HomeControllerTests
{
    [Fact]
    public void Index_ReturnsViewResult()
    {
        // Arrange
        var controller = new HomeController();

        // Act
        var result = controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void About_ReturnsViewResultWithModel()
    {
        // Arrange
        var controller = new HomeController();

        // Act
        var result = controller.About();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<WelcomeViewModel>(viewResult.ViewData.Model);
        Assert.NotNull(model);
        Assert.Contains("Welcome", model.Message);
    }

    [Fact]
    public void Privacy_ReturnsViewResult()
    {
        // Arrange
        var controller = new HomeController();

        // Act
        var result = controller.Privacy();

        // Assert
        Assert.IsType<ViewResult>(result);
    }
}
