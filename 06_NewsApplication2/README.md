# 🚀 06_NewsApplication2

Welcome to **NewsApplication2**. This unit demonstrates a modern ASP.NET Core 10 application using Domain-Driven Design (DDD), Asynchronous Programming, and a custom Tailwind CSS 4 frontend.

## 🎯 Learning Objectives
- [x] **Asynchronous Programming**: Deep dive into `async/await` patterns based on the "Tutorial Asynchrone Programmierung.pdf".
- [x] **DDD (Domain-Driven Design)**: Logical separation of Domain, Infrastructure, and Presentation layers.
- [x] **TDD (Test-Driven Development)**: Implementing core logic via Red-Green-Refactor cycles.
- [x] **Modern Frontend**: Building premium UIs with **Tailwind CSS 4.1.13** and **Font Awesome 7.2.0**.

## 🏗️ Architecture
- **Domain**: Pure C# entities and repository interfaces (`IArticleRepository`, `IAuthorRepository`).
- **Infrastructure**: EF Core 10 implementation, `AppDbContext`, and concrete repositories.
- **Presentation**: Asynchronous MVC Controllers and Razor Views styled with Tailwind.

## 📂 Folder Structure
- [**/src**](./src/README.md): Application source code.
- [**/tests**](./tests/README.md): Unit and Integration tests (xUnit).

---
> [!IMPORTANT]
> Every unit in this repository is designed to be a standalone, fully testable component. Ensure you check both the source and the tests to understand the full context.
