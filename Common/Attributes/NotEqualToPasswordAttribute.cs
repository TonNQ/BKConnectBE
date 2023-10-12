using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BKConnectBE.Common.Attributes
{
    public class NotEqualToPasswordAttribute : ValidationAttribute
    {
        private readonly string _currentPasswordPropertyName;

        public NotEqualToPasswordAttribute(string currentPasswordPropertyName)
        {
            _currentPasswordPropertyName = currentPasswordPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo currentPasswordProperty = validationContext.ObjectType.GetProperty(_currentPasswordPropertyName);

            var currentPasswordValue = currentPasswordProperty.GetValue(validationContext.ObjectInstance)?.ToString();
            var newPasswordValue = value?.ToString();

            if (currentPasswordValue == newPasswordValue)
            {
                return new ValidationResult("Mật khẩu mới không được trùng mật khẩu cũ");
            }

            return ValidationResult.Success;
        }
    }
}