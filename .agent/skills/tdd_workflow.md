# Skill: TDD Workflow (Red-Green-Refactor)

A structured approach to developing features with reliability.

## Step 1: RED (Write a failing test)

- Identify the requirement.
- Create a test class in the `tests/` project corresponding to the domain model.
- Write a test that fails (or doesn't compile).
- **Run the test**: Verify it fails for the right reason.

## Step 2: GREEN (Make it pass)

- Write the *minimum* amount of code in the `src/` project to make the test pass.
- Don't worry about perfection yet.
- **Run the test**: Verify it passes.

## Step 3: REFACTOR (Make it better)

- Review the implementation.
- Apply Clean Code principles (DRY, KISS, SOLID).
- Ensure documentation is complete (`/// <summary>`).
- **Run all tests**: Ensure no regressions.

## Excellence Checklist

- [ ] Is the code tested for all edge cases?
- [ ] Are naming conventions followed?
- [ ] Is every public member documented?
- [ ] Does it adhere to the domain rules (DDD)?
