# C# 14 & .NET 10 Best Practices

We strive for modern, efficient, and clean C# code.

## C# 14 Highlights

- **Primary Constructors (C# 12+)**: Preferred for all classes and structs,
  especially for Dependency Injection.
- **Collection Expressions (C# 12+)**: Use `[]` for all collection
  initializations: `List<int> numbers = [1, 2, 3];`.
- **Raw String Literals**: Use `"""` for multi-line strings, especially for SQL,
  JSON, or HTML.
- **Field Keyword (C# 14 Preview/Final)**: Use the `field` keyword in properties
  to avoid explicit backing fields where possible.
- **Span`T` & Memory`T`**: Prioritize these for performance-critical string or
  array manipulations.

## .NET 10.0.3 Features

- **Frozen Collections**: Use `ToFrozenDictionary()` and `ToFrozenSet()` for
  read-only collections that are frequently accessed but rarely changed.
- **SearchValues`T`**: Use for efficient searching within strings or spans.
- **LINQ Performance**: Leverage the latest LINQ optimizations for `Count()`,
  `First()`, and `ToList()`.

## Clean Code & IHK Standards

- **XML Dokumentation**: Jeder öffentliche Member MUSS detailliert auf Deutsch
  dokumentiert werden. Siehe
  [csharp_xml_documentation.md](file:///.agent/rules/csharp_xml_documentation.md).
- **Einhaltung IHK-Standards**: Fokus auf Fachbegriffe und klare Erklärungen für
  Anfänger.
- **One Class Per File**: Strictly enforced.
- **File-Scoped Namespaces**: Required: `namespace MyProject.Domain;`.
- **Primary Constructors**: For DI, use primary constructors to keep the code
  concise.

## Coding Style

- **Expression-Bodied Members**: Use for simple getters, methods, and
  properties.
- **Var Keyword**: Use `var` when the type is obvious on the right side:
  `var list = new List<string>();`.
- **Null Safety**: Enable `<Nullable>enable</Nullable>` and handle nulls
  explicitly using `??`, `?.`, and `!`.
