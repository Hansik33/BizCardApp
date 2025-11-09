using BizCardApp.Enums.ValidationResults;
using BizCardApp.Enums.ValidationResults.Optional;
using BizCardApp.Enums.ValidationResults.Required;

namespace BizCardApp.Validators.Results;

public class BusinessCardValidationOutcome
{
    public BusinessCardValidationResult Result { get; set; }

    public FirstNameValidationResult? FirstNameError { get; set; }
    public LastNameValidationResult? LastNameError { get; set; }
    public FullNameValidationResult? FullNameError { get; set; }

    public CompanyValidationResult? CompanyError { get; set; }
}