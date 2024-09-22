using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

public static class ValidationHelper
{
    public static void ValidateModel(object model, ControllerBase controller)
    {
        var validationContext = new ValidationContext(model, null, null);
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(model, validationContext, validationResults, true);

        foreach (var validationResult in validationResults)
        {
            controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage ?? "");
        }
    }
}
