# 🛡️ Unit 02: Hello MVC - Testing Infrastructure (`/tests`)

This directory contains the **Unit Tests** for the `HomeController`, ensuring that our MVC logic is correct.

## 🧪 Testing Strategy: Unit Testing
Unlike Unit 01, which focused on Integration tests, this unit emphasizes **Unit Tests** to verify individual components in isolation.

### 🛠️ Tooling
- **xUnit**: The primary testing framework.
- **Microsoft.NET.Test.Sdk**: Core testing library.

### 🔬 Test Cases
`HomeControllerTests.cs` verifies:
1. **Index Action**: Ensures the index root returns a valid `ViewResult`.
2. **Privacy Action**: Confirms that the privacy page returns its respective view.

## 🏃 How to Run Tests
From the folder or project root:
```bash
dotnet test
```

> [!TIP]
> Unit tests are fast and precise. They are your first line of defense against regressions in UI logic.
