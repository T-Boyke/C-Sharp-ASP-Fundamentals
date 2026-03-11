# Razor View & Partial Standards

Defines conventions and best practices for creating and using Razor Views and
Partial Views in ASP.NET Core MVC and Razor Pages.

## 1. Purpose of Partial Views

Partial Views are used to encapsulate and reuse UI elements, reducing code
duplication and keeping main views clean.

## 2. Typical Use Cases

- **Layout Components**: Header (`_Navigation.cshtml`), Footer
  (`_Footer.cshtml`), Sidebar (`_Sidebar.cshtml`).
- **Data Display (Iterative)**: Item cards (`_ProductCard.cshtml`), comments
  (`_Comment.cshtml`), or row details (`_TableRowPartial.cshtml`).
- **Form Elements**: Reusable forms (`_LoginForm.cshtml`), search bars
  (`_SearchBar.cshtml`), or standard validation scripts
  (`_ValidationScriptsPartial.cshtml`).
- **Feedback & State**: Dynamic alerts (`_StatusMessage.cshtml`) or loading
  indicators (`_LoadingSpinner.cshtml`).

## 3. Conventions

### Naming

- **Underscore Prefix**: Partial View filenames MUST start with an underscore
  (e.g., `_MyPartial.cshtml`) to distinguish them from full views/pages.
- **PascalCase**: Use PascalCase for the descriptive part of the name.

### Location

- **Local Partials**: If a partial is only used by a single controller/folder,
  place it in that specific `Views/[Controller]` directory.
- **Global Partials**: If a partial is shared across multiple areas, place it in
  `Views/Shared` or `Pages/Shared`.

## 4. Implementation (Tag Helpers)

Prefer the **Partial Tag Helper** over `@Html.PartialAsync` for better
readability and performance.

```html
<!-- Example: Rendering a product card in a loop -->
@foreach (var product in Model.Products) {
<partial name="_ProductCard" model="product" />
}
```

## 5. View Components

For more complex logic (e.g., dynamic sidebar content from a database), consider
using **View Components** instead of simple Partial Views to maintain a clean
separation of concerns.

## 6. Accessibility (BTHG/ARIA) & Privacy (DSGVO)

- **Semantic HTML**: Always use structural tags (`nav`, `main`, `aside`,
  `section`) rather than nested `div` loops.
- **ARIA & Keyboard Navigation**: Ensure custom UI widgets support
  `tabindex="0"`, have visible `focus-visible` UI states, and apply
  `aria-hidden="true"` to strictly decorative elements like FontAwesome icons.
- **Privacy / CDNs**: Never link CDNs directly inside Razor Views (e.g.,
  `<script src="https://cdn...">`). All static dependencies MUST be completely
  localized using LibMan for strict DSGVO compliance.
