using BizCardApp.Enums.ValidationResults.Required;
using BizCardApp.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace BizCardApp.Validators.Required;

public static class FullNameValidator
{
    public static FullNameValidationResult Validate(string fullName, IEnumerable<BusinessCardViewModel> businessCards)
    {
        if (businessCards.Count(businessCard => businessCard.FullName == fullName) > 1)
            return FullNameValidationResult.NotUnique;
        return FullNameValidationResult.Valid;
    }
}