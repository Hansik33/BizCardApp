using BizCardApp.Enums.ValidationResults.Optional;
using System.Linq;

namespace BizCardApp.Validators.Optional;

public static class JobTitleValidator
{
    public static JobTitleValidationResult Validate(string? jobTitle)
    {
        if (string.IsNullOrWhiteSpace(jobTitle))
            return JobTitleValidationResult.NotProvided;

        var value = jobTitle.Trim();

        if (value.Length < 2)
            return JobTitleValidationResult.TooShort;

        if (value.Length > 80)
            return JobTitleValidationResult.TooLong;

        static bool IsAllowed(char character) =>
            char.IsLetterOrDigit(character)
            || char.IsWhiteSpace(character)
            || character is '&' or '-' or '.' or ',' or '/' or '(' or ')' or '\'' or '+' or '#';

        if (!value.All(IsAllowed))
            return JobTitleValidationResult.InvalidCharacters;

        return JobTitleValidationResult.Valid;
    }
}