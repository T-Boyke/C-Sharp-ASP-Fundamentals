# Git & Commit Conventions

We strive for atomic, clean, and descriptive commit history.

## Atomic Commits

- One commit per logical change.
- Do not mix refactors with new features.
- Ensure the project builds and tests pass for every commit.

## Commit Naming (Conventional Commits)

Format: `<type>(<scope>): <description>`

### Types

- **feat**: A new feature.
- **fix**: A bug fix.
- **refactor**: Code change that neither fixes a bug nor adds a feature.
- **docs**: Documentation only changes.
- **style**: Changes that do not affect the meaning of the code (white-space, formatting, etc).
- **test**: Adding missing tests or correcting existing tests.
- **chore**: Changes to the build process or auxiliary tools and libraries.

### Guidelines

- Use the imperative, present tense: "change", not "changed" or "changes".
- No period at the end of the subject line.
- Scope should refer to the Unit or Project (e.g., `feat(04_ShoppingList): ...`).

- Keep branches short-lived.

## Workflow (Daily Sync)

To avoid conflicts and ensure environment stability, follow this sequence:

1. `git fetch`: Check for remote changes without merging.
2. `git status`: Review local state.
3. `git pull --tags origin main`: Always sync the latest state including tags.
4. `git push origin main:main`: Direct pushes to main are allowed for educational units, but verify stability first.

## Troubleshooting

### File Locks (Windows)

If you encounter `The process cannot access the file ... because it is being used by another process`:

- **Visual Studio**: Close the relevant `.csproj` or the entire IDE instance.
- **Background Processes**: Ensure no `dotnet` or `tailwindcss` processes are hung in the background.
- **Retry**: Use `dotnet build` again after closing the blocking application.
