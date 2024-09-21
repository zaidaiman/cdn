using System.ComponentModel.DataAnnotations;

public class NotEmptyIfNotNullAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is List<string> list && list.Count == 0)
        {
            return new ValidationResult("The list cannot be empty if it has a value.");
        }

        return ValidationResult.Success!;
    }
}
