using System;

namespace _05_NewsApplication.Domain.ValueObjects {
    public record Headline {
        public string Value { get; init; }

        public Headline(string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentException("Headline cannot be empty.");
            }

            if (value.Length > 200) {
                throw new ArgumentException("Headline cannot be longer than 200 characters.");
            }

            Value = value;
        }
    }
}
