# 🛠️ Unit 02: Hello MVC - Source Code (`/src`)

This directory contains the source code for the "Hello MVC" application, demonstrating the classic MVC pattern in ASP.NET Core 10.

## 🎯 Implementation Details

### 🏗️ MVC Components
- **Models**: `ErrorViewModel.cs` handles diagnostic data for views.
- **Views**: Razor templates (`.cshtml`) implementing the user interface.
- **Controllers**: `HomeController.cs` manages the interaction between models and views.

### 🌐 Routing Logic
```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```
This ensures the `Index` action of## 🏗️ Technical Overview: The MVC Lifecycle

This project demonstrates the **Model-View-Controller** pattern, which is the cornerstone of ASP.NET Core web development.

### � The Request Flow
1. **Request**: The browser sends a GET request to `/Home/About`.
2. **Routing**: ASP.NET Core matches the URL to the `HomeController` and `About` action.
3. **Controller** (`HomeController.cs`):
   - The action method is executed.
   - A **Model** (`WelcomeViewModel.cs`) is instantiated and populated with data.
   - The controller calls `return View(model)`, selecting the view and passing the data.
4. **View** (`About.cshtml`):
   - The Razor engine processes the view.
   - Model data is injected into the HTML using the `@Model` property.
5. **Response**: The final HTML is sent back to the browser.

## 🛠️ Components
- **Models**: [WelcomeViewModel.cs](./Models/WelcomeViewModel.cs) - Represents the data structure.
- **Views**: [About.cshtml](./Views/Home/About.cshtml) - The user interface template.
- **Controllers**: [HomeController.cs](./Controllers/HomeController.cs) - The glue that connects everything.

## 🚀 How to Run
1. From this directory: `dotnet run`
2. Navigate to: `https://localhost:[port]/Home/About`
