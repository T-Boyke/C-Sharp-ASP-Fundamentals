using Microsoft.EntityFrameworkCore;
using _07_Patienten.Domain.Entities;
using _07_Patienten.Infrastructure.Data;
using _07_Patienten.Infrastructure.Repositories;
using Xunit;

namespace _07_Patienten.Tests.Infrastructure;

public class RepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task PatientRepository_AddAndGet_ShouldWork()
    {
        // Arrange
        using var context = GetDbContext();
        var repo = new PatientRepository(context);
        var patient = new Patient { Firstname = "John", Lastname = "Doe", Birthdate = DateTime.Today, SocialSecurityNumber = "1234567890" };

        // Act
        await repo.AddAsync(patient);
        var result = await repo.GetByIdAsync(patient.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.Firstname);
    }

    [Fact]
    public async Task PatientRepository_GetAll_ShouldReturnOrderedList()
    {
        // Arrange
        using var context = GetDbContext();
        var repo = new PatientRepository(context);
        await repo.AddAsync(new Patient { Firstname = "Z", Lastname = "Z", Birthdate = DateTime.Today, SocialSecurityNumber = "0000000001" });
        await repo.AddAsync(new Patient { Firstname = "A", Lastname = "A", Birthdate = DateTime.Today, SocialSecurityNumber = "0000000000" });

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("A", result.First().Lastname);
    }

    [Fact]
    public async Task PatientRepository_Delete_ShouldRemovePatient()
    {
        // Arrange
        using var context = GetDbContext();
        var repo = new PatientRepository(context);
        var patient = new Patient { Firstname = "John", Lastname = "Doe", Birthdate = DateTime.Today, SocialSecurityNumber = "1234567890" };
        await repo.AddAsync(patient);

        // Act
        await repo.DeleteAsync(patient.Id);
        var result = await repo.GetByIdAsync(patient.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ExaminationRepository_AddAndGetByPatient_ShouldWork()
    {
        // Arrange
        using var context = GetDbContext();
        var examRepo = new ExaminationRepository(context);
        var exam = new Examination { PatientId = 1, Findings = "Test result", Date = DateTime.Now };

        // Act
        await examRepo.AddAsync(exam);
        var result = await examRepo.GetByPatientIdAsync(1);

        // Assert
        Assert.Single(result);
        Assert.Equal("Test result", result.First().Findings);
    }
}
