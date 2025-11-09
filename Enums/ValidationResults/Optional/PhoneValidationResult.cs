namespace BizCardApp.Enums.ValidationResults.Optional;

public enum PhoneValidationResult
{
    Valid,
    NotProvided,
    TooShort,
    TooLong,
    InvalidCharacters,
    InvalidFormat
}