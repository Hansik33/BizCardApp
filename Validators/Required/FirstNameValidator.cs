using BizCardApp.Enums.ValidationResults.Required;
using System.Linq;

namespace BizCardApp.Validators.Required;

public static class FirstNameValidator
{
    public static FirstNameValidationResult Validate(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return FirstNameValidationResult.Empty;

        if (firstName.Length < 2)
            return FirstNameValidationResult.TooShort;

        if (firstName.Length > 50)
            return FirstNameValidationResult.TooLong;

        if (!firstName.All(char.IsLetter))
            return FirstNameValidationResult.InvalidCharacters;

        return FirstNameValidationResult.Valid;
    }
}