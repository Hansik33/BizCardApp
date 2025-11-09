using BizCardApp.Enums.ValidationResults.Optional;
using System.Linq;
using System.Text.RegularExpressions;

namespace BizCardApp.Validators.Optional;

public static partial class AddressValidator
{
    private const int MinLength = 3;
    private const int MaxLength = 120;
    private static readonly char[] charArray = [',', '.', '/', '\\', ':', ';', '#'];

    [GeneratedRegex(@"\s+", RegexOptions.Compiled)]
    private static partial Regex MultiWhitespaceRegex();

    public static AddressValidationResult Validate(string? address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return AddressValidationResult.NotProvided;

        var value = address.Trim();

        if (value.Length < MinLength)
            return AddressValidationResult.TooShort;

        if (value.Length > MaxLength)
            return AddressValidationResult.TooLong;

        if (value.Any(character => character is '\r' or '\n'))
            return AddressValidationResult.InvalidCharacters;

        foreach (var character in value)
        {
            if (char.IsLetterOrDigit(character))
                continue;

            if (char.IsWhiteSpace(character))
                continue;

            if (IsAllowedPunctuation(character))
                continue;

            return AddressValidationResult.InvalidCharacters;
        }

        if (IsHardSeparator(value[0]) || IsHardSeparator(value[^1]))
            return AddressValidationResult.InvalidFormat;

        bool previousWasHardSeparator = false;
        foreach (var character in value)
        {
            if (IsHardSeparator(character))
            {
                if (previousWasHardSeparator)
                    return AddressValidationResult.InvalidFormat;
                previousWasHardSeparator = true;
            }
            else if (!char.IsWhiteSpace(character))
            {
                previousWasHardSeparator = false;
            }
        }

        if (value.StartsWith(". ") || value.StartsWith(", ") || value.EndsWith(" .") || value.EndsWith(" ,"))
            return AddressValidationResult.InvalidFormat;

        if (!value.Any(character => char.IsLetterOrDigit(character)))
            return AddressValidationResult.InvalidFormat;

        return AddressValidationResult.Valid;
    }

    private static bool IsAllowedPunctuation(char character) =>
        character is '.' or ',' or '-' or '–' or '—' or '/' or '\\' or '\'' or '’' or '"' or '#' or '(' or ')' or ':' or ';';

    private static bool IsHardSeparator(char character) =>
        character is '.' or ',' or '-' or '–' or '—' or '/' or '\\' or ':' or ';' or '#';

    public static string? Normalize(string? address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return null;

        var normalized = address.Trim();
        normalized = normalized.Replace('–', '-').Replace('—', '-').Replace('’', '\'');
        normalized = MultiWhitespaceRegex().Replace(normalized, " ");
        normalized = TrimAroundSeparators(normalized, charArray);
        normalized = MultiWhitespaceRegex().Replace(normalized, " ").Trim();
        return normalized;
    }

    private static string TrimAroundSeparators(string input, char[] separators)
    {
        var output = input;

        foreach (var separator in separators)
        {
            var pattern = $@"\s*\{separator}\s*";
            output = Regex.Replace(output, pattern, $"{separator}");

            if (separator is '.' or ',')
            {
                output = Regex.Replace(output, $@"\{separator}(?=\S)", $"{separator} ");
                output = Regex.Replace(output, $@"\{separator}\s{{2,}}", $"{separator} ");
            }
        }

        output = MultiWhitespaceRegex().Replace(output, " ").Trim();
        return output;
    }
}