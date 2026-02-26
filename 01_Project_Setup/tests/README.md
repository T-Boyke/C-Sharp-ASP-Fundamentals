# 🛡️ Unit 01: Project Setup - Testing Infrastructure (`/tests`)

This directory contains the **Integration Tests** for Unit 01, ensuring that the project setup is valid and the environment is healthy.

## 🧪 Testing Strategy: TDD & Integration
Following our **Clean Architecture** and **TDD** guidelines, we use system-level integration tests to verify the entire middleware pipeline.

### 🛠️ Tooling
- **xUnit**: The primary testing framework.
- **Microsoft.AspNetCore.Mvc.Testing**: Provides the `WebApplicationFactory` used to bootstrap the application in-memory for lightning-fast integration testing.

### 🔬 Test Cases
`ProjectSetupTests.cs` verifies the following:
1. **Endpoint Availability**: Ensures `/version` is reachable.
2. **Framework Alignment**: Confirms the application is running on **ASP.NET Core 10**.
3. **Payload Integrity**: Asserts that the JSON response contains the expected `Status: Healthy`.

## 🏃 How to Run Tests
From the project root or this folder:
```bash
dotnet test
```

## 📐 Design Patterns
- **IClassFixture**: Used to share the `WebApplicationFactory` instance across test methods for efficiency.
- **POCO Responses**: Uses `VersionResponse.cs` to deserialize results, keeping tests strongly typed and readable.

> [!TIP]
> Integration tests are your "Quality Gates". Always run them before pushing to ensure the foundation remains solid.
