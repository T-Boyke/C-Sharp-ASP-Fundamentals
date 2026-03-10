using _08_Patienten2.Application.DTOs;
using _08_Patienten2.Application.Interfaces;
using _08_Patienten2.Web.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace _08_Patienten2.Web.Tests;

public class PatientControllerTests
{
    private readonly IPatientService _service = Substitute.For<IPatientService>();
    private readonly PatientController _controller;

    public PatientControllerTests()
    {
        _controller = new PatientController(_service);
    }

    [Fact]
    public async Task Index_ShouldReturnViewWithPatients()
    {
        // Arrange
        var patients = new List<PatientDto> { new PatientDto(1, "A", "B", "A B", DateTime.Today, "123", 30, null) };
        _service.GetAllPatientsAsync().Returns(patients);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeAssignableTo<IEnumerable<PatientDto>>().Subject;
        model.Should().HaveCount(1);
    }

    [Fact]
    public void Create_ShouldReturnView()
    {
        // Act
        var result = _controller.Create();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }
}
