using BizCardApp.Enums.ValidationResults.Optional;
using System.Linq;

namespace BizCardApp.Validators.Optional;

public static class CompanyValidator
{
    public static CompanyValidationResult Validate(string? company)
    {
        if (string.IsNullOrWhiteSpace(company))
            return CompanyValidationResult.NotProvided;

        var value = company.Trim();

        if (value.Length < 2)
            return CompanyValidationResult.TooShort;

        if (value.Length > 120)
            return CompanyValidationResult.TooLong;

        static bool IsAllowed(char character) =>
            char.IsLetterOrDigit(character)
            || char.IsWhiteSpace(character)
            || character is '&' or '-' or '.' or ',' or '/' or '(' or ')' or '\'';

        if (!value.All(IsAllowed))
            return CompanyValidationResult.InvalidCharacters;

        return CompanyValidationResult.Valid;
    }
}