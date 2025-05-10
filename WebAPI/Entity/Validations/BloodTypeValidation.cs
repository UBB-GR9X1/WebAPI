using System.ComponentModel.DataAnnotations;
using System.Linq;

public class BloodTypeValidationAttribute : ValidationAttribute
{
    private static readonly string[] _allowed_types =
        { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string str && _allowed_types.Contains(str))
            return ValidationResult.Success;

        return new ValidationResult($"Invalid blood type. Allowed values: {string.Join(", ", _allowed_types)}");
    }
}
