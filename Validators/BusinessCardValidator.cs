using BizCardApp.Enums.ValidationResults;
using BizCardApp.Enums.ValidationResults.Required;
using BizCardApp.Validators.Required;
using BizCardApp.Validators.Results;
using BizCardApp.ViewModels;
using System.Collections.Generic;

namespace BizCardApp.Validators;

public static class BusinessCardValidator
{
    public static BusinessCardValidationOutcome Validate(BusinessCardViewModel businessCard)
    {
        var outcome = new BusinessCardValidationOutcome
        {
            Result = BusinessCardValidationResult.Success
        };

        var firstNameResult = FirstNameValidator.Validate(businessCard.FirstName);
        if (firstNameResult != FirstNameValidationResult.Valid)
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.FirstNameError = firstNameResult;
            return outcome;
        }

        var lastNameResult = LastNameValidator.Validate(businessCard.LastName);
        if (lastNameResult != LastNameValidationResult.Valid)
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.LastNameError = lastNameResult;
            return outcome;
        }

        return outcome;
    }

    public static BusinessCardValidationOutcome Validate(BusinessCardViewModel businessCard,
                                                         IEnumerable<BusinessCardViewModel> businessCards)
    {
        var outcome = new BusinessCardValidationOutcome
        {
            Result = BusinessCardValidationResult.Success
        };

        var firstNameResult = FirstNameValidator.Validate(businessCard.FirstName);
        if (firstNameResult != FirstNameValidationResult.Valid)
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.FirstNameError = firstNameResult;
            return outcome;
        }

        var lastNameResult = LastNameValidator.Validate(businessCard.LastName);
        if (lastNameResult != LastNameValidationResult.Valid)
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.LastNameError = lastNameResult;
            return outcome;
        }

        var fullNameResult = FullNameValidator.Validate(businessCard.FullName, businessCards);
        if (fullNameResult != FullNameValidationResult.Valid)
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.FullNameError = fullNameResult;
            return outcome;
        }

        return outcome;
    }
}