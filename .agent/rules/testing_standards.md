# Testing Standards

Qualitativ hochwertige Tests sind das Rückgrat jeder exzellenten Anwendung.

## 🏗️ Struktur (AAA-Pattern)

Jeder Test muss strikt nach dem **Arrange-Act-Assert** Pattern gegliedert sein:

1.  **Arrange**: Vorbereiten der Testdaten, Mocks und des "System Under Test" (SUT).
2.  **Act**: Ausführen der zu testenden Aktion.
3.  **Assert**: Überprüfen des Ergebnisses.

## 🏷️ Benennungskonvention

Format: `UnitOfWork_StateUnderTest_ExpectedBehavior`
Beispiel: `BerechneGesamtpreis_RabattGültig_GibtkorrektenBetragZurück`

## 🛠️ Werkzeuge & Best Practices

- **FluentAssertions**: Nutze `result.Should().Be(expected);` statt Standard-Asserts für bessere Lesbarkeit.
- **xUnit**: Bevorzugtes Test-Framework. Nutze `[Fact]` für einfache Tests und `[Theory]` für datengesteuerte Tests via `[InlineData]`.
- **NSubstitute / Moq**: Nutze Mocks nur für externe Abhängigkeiten (DB, API, Services). Logik sollte niemals in Mocks stecken.
- **Constructor Injection**: Injiziere Abhängigkeiten via Constructor in das SUT, um einfaches Mocking zu ermöglichen.

## 🛡️ Excellence Checklist
- [ ] Folgt der Test dem AAA-Pattern?
- [ ] Ist der Name präzise und beschreibt das erwartete Verhalten?
- [ ] Wird gegen ein Interface getestet (Decoupling)?
- [ ] Sind alle Assertions aussagekräftig (Fluent)?
- [ ] Ist der Test unabhängig von anderen Tests?
