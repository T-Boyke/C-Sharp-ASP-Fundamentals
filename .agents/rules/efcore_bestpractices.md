# Entity Framework Core 10 Best Practices

High-performance data access with EF Core 10.

## Core Principles

- **Separation of Entities and Models**: Domain entities should remain clean; use DTOs or ViewModels for data transfer.
- **Tracking vs No-Tracking**: Use `.AsNoTracking()` for read-only operations to improve performance.
- **Compiled Models**: For large models, use `dotnet ef dbcontext optimize` to generate compiled models.

## Mapping & Schema

- **Shadow Properties**: Use for metadata like `CreatedAt`, `LastModifiedBy`.
- **Value converters**: Map enums and value objects correctly.
- **Global Query Filters**: Implement soft-delete patterns using global filters.
- **Interceptors**: Use interceptors for auditing or logging.

## Performance Excellence

- **Batching**: EF 10 handles batching efficiently by default; avoid manually splitting batches unless necessary.
- **Split Queries**: Use `.AsSplitQuery()` for large results with many includes to avoid Cartesian explosion.
- **JSON Columns**: Utilize native JSON support for semi-structured data.

## DDD Integration

- **Owned Types**: Use for Value Objects.
- **Private Setters**: Entities should have private setters to ensure domain logic encapsulated in methods is used.
- **Encapsulated Collections**: Use `public IEnumerable<Child> Children => _children.AsReadOnly();` with a private backing field for collection navigation.
