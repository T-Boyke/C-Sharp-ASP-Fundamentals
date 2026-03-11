# Markdown Linting Guidelines

To maintain excellence in documentation, all markdown files MUST adhere to these rules.

## Core Rules

- **MD013 (Line Length)**: Maximum 120 characters per line. Exceptions for tables and long URLs.
- **MD033 (No Inline HTML)**: Avoid HTML tags. Use standard Markdown. Exceptions for `<details>`/`<summary>` and `<p align="center">` are allowed only in `README.md`.
- **MD041 (First line Header)**: Every file must start with a top-level `#` header.
- **MD022 (Headers Blank Lines)**: Headers must be surrounded by blank lines.
- **MD032 (Lists Blank Lines)**: Lists must be surrounded by blank lines.

## Visual Excellence

- Use **GitHub Alerts** (`> [!NOTE]`, `> [!IMPORTANT]`, etc.) to highlight critical information.
- Use **Emojis** at the start of headers to improve scannability (e.g., `## 🚀 Features`).
- Use **Mermaid Diagrams** for complex architectures or logic flows.
  - **Compatibility**: Avoid `usecaseDiagram` or `activityDiagram`. Use `graph LR/TD` for architecture, flows, and logic to ensure rendering across all platforms (GitHub, IDEs).
- Tables should be used for comparisons or status overviews.

## Formatting

- Use `backticks` for file names, directories, and code symbols.
- Use **bold** for emphasis and important terms.
- Links should be descriptive: `[README.md](file:///path/to/README.md)`.
