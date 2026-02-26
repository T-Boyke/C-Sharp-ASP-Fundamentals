# 🛠️ Unit 03: RSVP App - Source Code (`/src`)

This directory contains the source code for the RSVP Application, demonstrating end-to-end form handling in ASP.NET Core 10.

## 🏗️ Technical Implementation

### 🛡️ The Model (`GuestResponse.cs`)
We use **Data Annotations** to define validation rules directly on the model:
```csharp
[Required(ErrorMessage = "Please enter your name")]
public string Name { get; set; }

[EmailAddress]
public string Email { get; set; }
```

### 🧠 The Repository (`Repository.cs`)
A static, in-memory collection used to store guest responses during the application's lifecycle. This demonstrates how to manage state across multiple requests without a database.

### 🎮 The Controller (`HomeController.cs`)
Demonstrates the **Overloading** of action methods to handle different HTTP verbs:
- `[HttpGet] RsvpForm()`: Displays the empty form.
- `[HttpPost] RsvpForm(GuestResponse response)`: Processes the submission and checks `ModelState.IsValid`.

## 🚀 How to Run
1. From this directory: `dotnet run`
2. Open the browser and visit the Home page.
3. Click "RSVP Now" to fill out the form.
