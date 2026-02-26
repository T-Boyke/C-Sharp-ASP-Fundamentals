# 🛠️ Unit 02: Hello MVC - Source Code (`/src`)

This directory contains the source code for the "Hello MVC" application, demonstrating the classic MVC pattern in ASP.NET Core 10.

## 🎯 Implementation Details

### 🏗️ MVC Components
- **Models**: `ErrorViewModel.cs` handles diagnostic data for views.
- **Views**: Razor templates (`.cshtml`) implementing the user interface.
- **Controllers**: `HomeController.cs` manages the interaction between models and views.

### 🌐 Routing Logic
The default routing is configured in `Program.cs`:
```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```
This ensures the `Index` action of `HomeController` is the entry point.

## 🚀 How to Run
From the root of the unit (`02_Hello_MVC/src`):
```bash
dotnet run
```
Then visit `https://localhost:[port]/` to see the application landing page.
