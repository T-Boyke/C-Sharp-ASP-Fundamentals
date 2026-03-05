using Xunit;
using _07_Patienten.Domain.Entities;

namespace _07_Patienten.Tests.Domain;

public class PatientTests
{
    [Fact]
    public void Age_ShouldCalculateCorrectly_WhenBirthdayHasPassed()
    {
        // Arrange
        var birthdate = DateTime.Today.AddYears(-30).AddDays(-1);
        var patient = new Patient 
        { 
            Firstname = "Max", 
            Lastname = "Mustermann", 
            Birthdate = birthdate,
            SocialSecurityNumber = "1234567890"
        };

        // Act
        var age = patient.Age;

        // Assert
        Assert.Equal(30, age);
    }

    [Fact]
    public void Age_ShouldCalculateCorrectly_WhenBirthdayHasNotPassedYet()
    {
        // Arrange
        var birthdate = DateTime.Today.AddYears(-30).AddDays(1);
        var patient = new Patient 
        { 
            Firstname = "Max", 
            Lastname = "Mustermann", 
            Birthdate = birthdate,
            SocialSecurityNumber = "1234567890"
        };

        // Act
        var age = patient.Age;

        // Assert
        Assert.Equal(29, age);
    }

    [Fact]
    public void Fullname_ShouldReturnFirstAndLastname()
    {
        // Arrange
        var patient = new Patient 
        { 
            Firstname = "Max", 
            Lastname = "Mustermann", 
            Birthdate = DateTime.Today,
            SocialSecurityNumber = "1234567890"
        };

        // Assert
        Assert.Equal("Max Mustermann", patient.Fullname);
    }
}
