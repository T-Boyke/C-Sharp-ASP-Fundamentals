# Frontend Asset Standards

Defines the requirements for managing and serving frontend assets (CSS, Fonts,
Icons) in the repository.

## 1. Local-Only Assets

- **NO EXTERNAL CDNs**: All assets (Bootstrap, Tailwind, Font Awesome, Fonts)
  MUST be served locally from `wwwroot/lib/` or `wwwroot/css/`.
- **Privacy & Performance**: This ensures compliance with privacy regulations
  (GDPR) and improves load performance by reducing external lookups.

## 2. Dependency Management

- **No Node.js / npm**: The repository strictly avoids `node_modules`. Asset
  management must be done via:
  - **LibMan**: For client-side libraries and fonts (prefer `unpkg` or `cdnjs`
    providers).
  - **NuGet**: For build tools like Tailwind CLI wrappers.

## 3. Technology Standards

- **Tailwind CSS**: Use version 4.2+. Integration MUST use `Tailwind.MSBuild`
  (e.g., v2.0.2) to automate compilation during `dotnet build`.
- **Icons**: Use **Font Awesome 7.2.0 Free** or newer.
- **Typography**: Use **Google Sans Flex** or standard system fonts. Variable
  fonts are preferred for "Flex" capabilities.

## 4. Project Configuration

- **Tailwind Setup**:
  - Input file: `wwwroot/css/input.css`
  - Output file: `wwwroot/css/site.css`
  - Properties: `<TailwindCSSInputFile>` and `<TailwindCSSOutputFile>` in the
    `.csproj`.
- **LibMan Setup**:
  - Configuration file: `libman.json` at the project root.
  - Assets MUST be restored to `wwwroot/lib/`.

- Follow the "Premium Design" guidelines: glassmorphism, subtle gradients, and
  modern typography as defined in the visual excellence workflow.

## 6. No Committed Binaries

- **BUILD-TIME BINARIES**: Binaries generated or downloaded during build (e.g.,
  `tailwindcss.exe`) MUST be ignored via `.gitignore` and NEVER committed to the
  repository.
- **EXECUTION POLICY**: These tools are restored automatically by native package
  managers (NuGet, LibMan) on the local machine.
