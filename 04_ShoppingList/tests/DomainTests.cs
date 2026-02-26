using System.Linq;
using _04_ShoppingList.Models;

namespace tests;

public class DomainTests
{
    [Fact]
    public void Position_ShouldHaveProperties()
    {
        // Assert that Position class has the expected properties
        var position = new Position
        {
            Name = "Milch",
            Anzahl = 2,
            Geschaeft = "Rewe"
        };

        Assert.Equal("Milch", position.Name);
        Assert.Equal(2, position.Anzahl);
        Assert.Equal("Rewe", position.Geschaeft);
    }

    [Fact]
    public void Repository_ShouldStorePositions()
    {
        // Arrange
        Repository.Clear(); // helper function we might need for testing
        var position1 = new Position { Name = "Milch", Anzahl = 2, Geschaeft = "Rewe" };
        var position2 = new Position { Name = "Brot", Anzahl = 1, Geschaeft = "Aldi" };

        // Act
        Repository.AddResponse(position1);
        Repository.AddResponse(position2);

        // Assert
        var positions = Repository.Positions.ToList();
        Assert.Equal(2, positions.Count);
        Assert.Contains(position1, positions);
        Assert.Contains(position2, positions);
    }
}

