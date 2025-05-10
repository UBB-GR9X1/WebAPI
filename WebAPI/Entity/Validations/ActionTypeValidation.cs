using System.ComponentModel.DataAnnotations;
using System.Linq;

public class ActionTypeValidationAttribute : ValidationAttribute
{
    private static readonly string[] _allowed_values = new[]
    {
        "LOGIN", "LOGOUT", "UPDATE_PROFILE", "CHANGE_PASSWORD", "DELETE_ACCOUNT", "CREATE_ACCOUNT"
    };

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string str && _allowed_values.Contains(str))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult($"Invalid ActionType. Allowed values: {string.Join(", ", _allowed_values)}");
    }
}