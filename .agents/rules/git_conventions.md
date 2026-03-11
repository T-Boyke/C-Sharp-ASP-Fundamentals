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

## Branching

- Use descriptive branch names: `feature/xyz`, `bugfix/abc`, `refactor/clean-up`.
- Keep branches short-lived.
