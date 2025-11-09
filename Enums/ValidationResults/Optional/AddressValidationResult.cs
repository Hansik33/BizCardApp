namespace BizCardApp.Enums.ValidationResults.Optional;

public enum AddressValidationResult
{
    Valid,
    NotProvided,
    TooShort,
    TooLong,
    InvalidCharacters,
    InvalidFormat
}