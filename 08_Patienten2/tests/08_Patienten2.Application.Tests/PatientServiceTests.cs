using _08_Patienten2.Application.DTOs;
using _08_Patienten2.Application.Interfaces;
using _08_Patienten2.Application.Services;
using _08_Patienten2.Domain.Entities;
using _08_Patienten2.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace _08_Patienten2.Application.Tests;

public class PatientServiceTests
{
    private readonly IPatientRepository _repository = Substitute.For<IPatientRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IPatientService _service;

    public PatientServiceTests()
    {
        _service = new PatientService(_repository, _unitOfWork);
    }

    [Fact]
    public async Task GetAllPatientsAsync_ShouldReturnDtos()
    {
        // Arrange
        var patients = new List<Patient> { new Patient("A", "B", DateTime.Today, "123") };
        _repository.GetAllAsync().Returns(patients);

        // Act
        var result = await _service.GetAllPatientsAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Firstname.Should().Be("A");
    }

    [Fact]
    public async Task CreatePatientAsync_ShouldCallRepositoryAndSave()
    {
        // Arrange
        var createDto = new PatientCreateDto("Max", "Mustermann", DateTime.Today, "1234567890");
        
        // Act
        var result = await _service.CreatePatientAsync(createDto);

        // Assert
        await _repository.Received(1).AddAsync(Arg.Any<Patient>());
        await _unitOfWork.Received(1).SaveChangesAsync();
    }
}
