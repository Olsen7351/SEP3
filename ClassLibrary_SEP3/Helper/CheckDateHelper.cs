using System.ComponentModel.DataAnnotations;

namespace BlazorAppTEST.Services
{
    public class CheckDateHelper : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Cast the input value to a DateTime object
            if (value is DateTime date)
            {
                // Compare the input date with today's date
                if (date.Date < DateTime.Today)
                {
                    // If the date is in the past, return a validation error
                    return new ValidationResult(ErrorMessage);
                }
            }
            else
            {
                // If the value is not a valid DateTime, return a validation error
                return new ValidationResult("Invalid date");
            }

            // If none of the above conditions are met, return success
            return ValidationResult.Success;
        }
    }
}