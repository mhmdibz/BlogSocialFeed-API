using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blog.Domain.ValueObjects
{
    public sealed class Email
    {
        private const string EmailPattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
        private static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(100);

        private Email(string value = "") => Value = value;

        public string Value { get; private init; }

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Email is required.", nameof(value));
            }

            var normalized = value.Trim().ToLowerInvariant();
            
            // Use Regex.IsMatch with timeout to prevent ReDoS attacks
            if (!Regex.IsMatch(normalized, EmailPattern, 
                RegexOptions.Compiled | RegexOptions.CultureInvariant, RegexTimeout))
            {
                throw new ArgumentException("Email format is invalid.", nameof(value));
            }

            return new Email(normalized);
        }

        public override bool Equals(object? obj) => obj is Email email && Value.Equals(email.Value, StringComparison.InvariantCultureIgnoreCase);

        public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();

        public static implicit operator string(Email email) => email.Value;
        public static explicit operator Email(string value) => Create(value);
        public override string ToString() => Value;
    }
}
