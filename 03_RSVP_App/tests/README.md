# 🛡️ Unit 03: RSVP App - Testing Infrastructure (`/tests`)

This directory contains the **Unit Tests** for the RSVP application, focusing on validation and controller behavior.

## 🧪 Testing Focus

### 🔬 GuestResponse Validation
Tests verify that our **Data Annotations** correctly catch invalid input, such as:
- Missing mandatory fields (Name, Email, etc.).
- Incorrectly formatted email addresses.

### 🎮 Controller Logic
Uses `Unit_03_RSVP_App.Tests` to verify:
1. **POST Success**: That valid submissions are correctly added to the repository and return the "Thanks" view.
2. **ModelState**: That invalid data remains on the form for user correction.

## 🏃 How to Run Tests
From this directory:
```bash
dotnet test
```

> [!TIP]
> Testing models with Data Annotations can be done via `Validator.TryValidateObject`, as demonstrated in `RsvpTests.cs`.
