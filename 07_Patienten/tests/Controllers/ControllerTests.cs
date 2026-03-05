using Microsoft.AspNetCore.Mvc;
using Moq;
using _07_Patienten.Controllers;
using _07_Patienten.Domain.Entities;
using _07_Patienten.Domain.Interfaces;
using Xunit;

namespace _07_Patienten.Tests.Controllers;

public class PatientsControllerTests
{
    private readonly Mock<IPatientRepository> _mockRepo;
    private readonly PatientsController _controller;

    public PatientsControllerTests()
    {
        _mockRepo = new Mock<IPatientRepository>();
        _controller = new PatientsController(_mockRepo.Object);
    }

    [Fact]
    public async Task Index_ReturnsViewWithPatients()
    {
        // Arrange
        var patients = new List<Patient> { new Patient { Firstname = "A", Lastname = "B", SocialSecurityNumber = "1", Birthdate = DateTime.Today } };
        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(patients);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Patient>>(viewResult.ViewData.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task Create_Post_RedirectsToIndex_WhenModelIsValid()
    {
        // Arrange
        var patient = new Patient { Firstname = "John", Lastname = "Doe", SocialSecurityNumber = "1234567890", Birthdate = DateTime.Today };

        // Act
        var result = await _controller.Create(patient);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        _mockRepo.Verify(r => r.AddAsync(patient), Times.Once);
    }

    [Fact]
    public async Task Details_ReturnsNotFound_WhenPatientDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetByIdWithExaminationsAsync(1)).ReturnsAsync((Patient?)null);

        // Act
        var result = await _controller.Details(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

public class ExaminationsControllerTests
{
    private readonly Mock<IExaminationRepository> _mockRepo;
    private readonly ExaminationsController _controller;

    public ExaminationsControllerTests()
    {
        _mockRepo = new Mock<IExaminationRepository>();
        _controller = new ExaminationsController(_mockRepo.Object);
    }

    [Fact]
    public async Task Create_Post_RedirectsToPatientDetails_WhenModelIsValid()
    {
        // Arrange
        var exam = new Examination { PatientId = 5, Findings = "OK", Date = DateTime.Now };

        // Act
        var result = await _controller.Create(exam);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Details", redirectToActionResult.ActionName);
        Assert.Equal("Patients", redirectToActionResult.ControllerName);
        Assert.Equal(5, redirectToActionResult.RouteValues?["id"]);
    }
}
