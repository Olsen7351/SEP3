using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ClassLibrary_SEP3.Helper
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidEndDateAttribute : ValidationAttribute
    {
        private string startDatePropertyName;
        private string endDatePropertyName;

        public ValidEndDateAttribute(string startDatePropertyName, string endDatePropertyName)
        {
            this.startDatePropertyName = startDatePropertyName;
            this.endDatePropertyName = endDatePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo startDateProperty = validationContext.ObjectType.GetProperty(startDatePropertyName);
            PropertyInfo endDateProperty = validationContext.ObjectType.GetProperty(endDatePropertyName);

            if (startDateProperty == null || endDateProperty == null)
            {
                throw new ArgumentException("Property not found.");
            }

            var startDate = (DateTime)startDateProperty.GetValue(value);
            var endDate = (DateTime)endDateProperty.GetValue(value);

            if (endDate < startDate)
            {
                return new ValidationResult("End date must be after the start date.");
            }

            return ValidationResult.Success;
        }
    }
}