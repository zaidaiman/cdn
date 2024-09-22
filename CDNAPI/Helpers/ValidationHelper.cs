using System.ComponentModel.DataAnnotations;

public class NotEmptyIfNotNullAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is List<string> list)
        {
            if (list.Count == 0) return new ValidationResult("The list cannot be empty if it has a value.");

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    return new ValidationResult("The list cannot be empty if it has a value.");
                }
            }
        }

        return ValidationResult.Success!;
    }
}
