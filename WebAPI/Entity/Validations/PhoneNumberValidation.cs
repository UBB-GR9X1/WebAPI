using System.ComponentModel.DataAnnotations;

public class PhoneNumberValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string phone = value as string;
        if (string.IsNullOrEmpty(phone) || phone.Length == 10)
            return ValidationResult.Success;

        return new ValidationResult("Phone number must be exactly 10 characters or left empty.");
    }
}
