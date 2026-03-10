using _08_Patienten2.Domain.Entities;
using _08_Patienten2.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace _08_Patienten2.Domain.Tests;

public class PatientDomainTests
{
    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Arrange
        var birthdate = new DateTime(1990, 1, 1);
        
        // Act
        var patient = new Patient("Max", "Mustermann", birthdate, "1234010190");

        // Assert
        patient.Firstname.Should().Be("Max");
        patient.Lastname.Should().Be("Mustermann");
        patient.Birthdate.Should().Be(birthdate);
        patient.SocialSecurityNumber.Should().Be("1234010190");
    }

    [Fact]
    public void Age_ShouldCalculateCorrectly()
    {
        // Arrange
        var birthdate = DateTime.Today.AddYears(-30);
        var patient = new Patient("Max", "Mustermann", birthdate, "1234567890");

        // Act & Assert
        patient.Age.Should().Be(30);
    }

    [Fact]
    public void UpdateAddress_ShouldChangeAddress()
    {
        // Arrange
        var patient = new Patient("Max", "Mustermann", DateTime.Today, "1234567890");
        var newAddress = new Address("Teststraße 1", "12345", "Teststadt");

        // Act
        patient.UpdateAddress(newAddress);

        // Assert
        patient.Address.Should().Be(newAddress);
    }
}
