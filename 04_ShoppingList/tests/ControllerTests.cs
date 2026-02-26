using Microsoft.AspNetCore.Mvc;
using System.Linq;
using _04_ShoppingList.Controllers;
using _04_ShoppingList.Models;

namespace tests;

public class ControllerTests
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
    public void ArtikelForm_Get_ReturnsViewResult()
    {
        // Arrange
        var controller = new HomeController();

        // Act
        var result = controller.ArtikelForm();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void ArtikelForm_Post_AddsPositionAndReturnsView()
    {
        // Arrange
        Repository.Clear();
        var controller = new HomeController();
        var position = new Position { Name = "Apfel", Anzahl = 5, Geschaeft = "Lidl" };

        // Act
        var result = controller.ArtikelForm(position);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Angelegt", viewResult.ViewName);
        Assert.Single(Repository.Positions);
    }

    [Fact]
    public void ArtikelAnsehen_ReturnsViewWithPositions()
    {
        // Arrange
        Repository.Clear();
        Repository.AddResponse(new Position { Name = "Tee", Anzahl = 1, Geschaeft = "Rewe" });
        var controller = new HomeController();

        // Act
        var result = controller.ArtikelAnsehen();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<System.Collections.Generic.IEnumerable<Position>>(viewResult.Model);
        Assert.Single(model);
    }
}

