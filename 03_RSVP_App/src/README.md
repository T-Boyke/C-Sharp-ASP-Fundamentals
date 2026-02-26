# 🛠️ Unit 03: RSVP App - Modern Frontend Architecture

This directory contains the refactored RSVP Application, demonstrating high-performance styling and modular UI components.

## 🏗️ Architectural Decisions

### 🎨 Tailwind CSS 4 & OOCSS
We use **Tailwind CSS 4** for styling, following the **Object-Oriented CSS** principle. Base "objects" are defined in `_Layout.cshtml` using `@layer components`:
- `.card`: Structure for content grouping.
- `.btn`: Standardized button behavior.
- `.input-field`: Unified form aesthetics.

### 🧩 SFC-Style Components (Partial Views)
To maintain **DRY** and **SOLID** principles, we've extracted repeated UI elements into modular components:
- `_FormControl.cshtml`: Encapsulates label, input, and validation message.
- `_Button.cshtml`: Standardized button component with theme support.

### 🛡️ Clean Code & SOLID
- **Single Responsibility**: Each component has one job (e.g., handling user input).
- **KISS**: We use simple Browser-based Tailwind integration for zero build-step overhead.
- **DRY**: Every form field and button in the app is driven by the same component logic.

## 🚀 How to Run
1. From this directory: `dotnet run`
2. Experience the premium, responsive UI at `https://localhost:[port]/`.
