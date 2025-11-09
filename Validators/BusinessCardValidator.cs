using BizCardApp.Enums.ValidationResults;
using BizCardApp.Enums.ValidationResults.Optional;
using BizCardApp.Enums.ValidationResults.Required;
using BizCardApp.Validators.Optional;
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
        if (firstNameResult is not FirstNameValidationResult.Valid)
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.FirstNameError = firstNameResult;
            return outcome;
        }

        var lastNameResult = LastNameValidator.Validate(businessCard.LastName);
        if (lastNameResult is not LastNameValidationResult.Valid)
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.LastNameError = lastNameResult;
            return outcome;
        }

        var companyResult = CompanyValidator.Validate(businessCard.Company);
        if (companyResult is not (CompanyValidationResult.Valid or CompanyValidationResult.NotProvided))
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.CompanyError = companyResult;
            return outcome;
        }

        var jobTitleResult = JobTitleValidator.Validate(businessCard.JobTitle);
        if (jobTitleResult is not (JobTitleValidationResult.Valid or JobTitleValidationResult.NotProvided))
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.JobTitleError = jobTitleResult;
            return outcome;
        }

        var phoneResult = PhoneValidator.Validate(businessCard.Phone);
        if (phoneResult is not (PhoneValidationResult.Valid or PhoneValidationResult.NotProvided))
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.PhoneError = phoneResult;
            return outcome;
        }

        var emailResult = EmailValidator.Validate(businessCard.Email);
        if (emailResult is not (EmailValidationResult.Valid or EmailValidationResult.NotProvided))
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.EmailError = emailResult;
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
        if (firstNameResult is not FirstNameValidationResult.Valid)
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.FirstNameError = firstNameResult;
            return outcome;
        }

        var lastNameResult = LastNameValidator.Validate(businessCard.LastName);
        if (lastNameResult is not LastNameValidationResult.Valid)
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.LastNameError = lastNameResult;
            return outcome;
        }

        var fullNameResult = FullNameValidator.Validate(businessCard.FullName, businessCards);
        if (fullNameResult is not FullNameValidationResult.Valid)
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.FullNameError = fullNameResult;
            return outcome;
        }

        var companyResult = CompanyValidator.Validate(businessCard.Company);
        if (companyResult is not (CompanyValidationResult.Valid or CompanyValidationResult.NotProvided))
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.CompanyError = companyResult;
            return outcome;
        }

        var jobTitleResult = JobTitleValidator.Validate(businessCard.JobTitle);
        if (jobTitleResult is not (JobTitleValidationResult.Valid or JobTitleValidationResult.NotProvided))
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.JobTitleError = jobTitleResult;
            return outcome;
        }

        var phoneResult = PhoneValidator.Validate(businessCard.Phone);
        if (phoneResult is not (PhoneValidationResult.Valid or PhoneValidationResult.NotProvided))
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.PhoneError = phoneResult;
            return outcome;
        }

        var emailResult = EmailValidator.Validate(businessCard.Email);
        if (emailResult is not (EmailValidationResult.Valid or EmailValidationResult.NotProvided))
        {
            outcome.Result = BusinessCardValidationResult.Failure;
            outcome.EmailError = emailResult;
            return outcome;
        }

        return outcome;
    }
}