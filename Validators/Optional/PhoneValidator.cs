using BizCardApp.Enums.ValidationResults.Optional;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BizCardApp.Validators.Optional;

public static partial class PhoneValidator
{
    [GeneratedRegex(@"^\+48[1-9]\d{8}$", RegexOptions.Compiled)]
    private static partial Regex E164PlRegex();

    public static PhoneValidationResult Validate(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return PhoneValidationResult.NotProvided;

        var value = phone.Trim();

        if (E164PlRegex().IsMatch(value))
            return PhoneValidationResult.Valid;

        if (!value.StartsWith("+48", StringComparison.Ordinal))
            return PhoneValidationResult.InvalidFormat;

        var rest = value.Length > 3 ? value[3..] : string.Empty;

        if (rest.Length < 9)
            return PhoneValidationResult.TooShort;

        if (rest.Length > 9)
            return PhoneValidationResult.TooLong;

        if (!rest.All(char.IsDigit))
            return PhoneValidationResult.InvalidCharacters;

        if (rest[0] == '0')
            return PhoneValidationResult.InvalidFormat;

        return PhoneValidationResult.InvalidFormat;
    }

    public static string? NormalizeToE164Like(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return null;

        var raw = phone.Trim();

        if (E164PlRegex().IsMatch(raw))
            return raw;

        var digits = new string([.. raw.Where(char.IsDigit)]);

        if (digits.StartsWith("48", StringComparison.Ordinal) && digits.Length == 11)
            return "+48" + digits[2..];

        if (digits.Length == 9)
            return "+48" + digits;

        return digits;
    }
}