using BizCardApp.Enums.ValidationResults.Required;
using System.Linq;

namespace BizCardApp.Validators.Required;

public static class LastNameValidator
{
    public static LastNameValidationResult Validate(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            return LastNameValidationResult.Empty;

        if (lastName.Length < 2)
            return LastNameValidationResult.TooShort;

        if (lastName.Length > 50)
            return LastNameValidationResult.TooLong;

        if (!lastName.All(char.IsLetter))
            return LastNameValidationResult.InvalidCharacters;

        return LastNameValidationResult.Valid;
    }
}