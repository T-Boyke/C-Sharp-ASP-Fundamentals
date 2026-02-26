# 🛠️ Unit 01: Project Setup - Source Code (`/src`)

This directory contains the core ASP.NET Core 10 Web Application for the initial project setup.

## 🎯 Implementation Details

The application is built using the latest **Minimal APIs** and **Top-Level Statements** in C# 14 / .NET 10.

### 🏗️ Application Bootstrap (`Program.cs`)
- **`WebApplicationBuilder`**: Used to configure services and the app's environment.
- **Service Collection**: `builder.Services.AddControllersWithViews()` is used to support MVC pattern later in the course.
- **Middleware Pipeline**:
  - `UseHttpsRedirection()`: Ensures secure communication.
  - `UseRouting()`: Enables endpoint matching.
  - `UseAuthorization()`: Standard authorization setup.

### 🧪 Verification Endpoint
We added a custom endpoint specifically for verified automated testing:
```csharp
app.MapGet("/version", () => Results.Ok(new { 
    Framework = "ASP.NET Core 10", 
    RuntimeVersion = Environment.Version.ToString(),
    Status = "Healthy"
}));
```
This endpoint allows the CI/CD pipeline and integration tests to confirm the environment is correctly initialized.

## 🚀 How to Run
From the root of the unit (`01_Project_Setup/src`):
```bash
dotnet run
```
Then visit `https://localhost:[port]/version` to see the health check.

## 📐 Architecture Note
We use `public partial class Program { }` at the end of `Program.cs` to make the entry point accessible to our integration test projects using `WebApplicationFactory`.
