using System.ComponentModel.DataAnnotations;

namespace CdDiskStoreAspNetCore.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PasswordValidation : ValidationAttribute
    {
        public PasswordValidation()
        {
            ErrorMessage = "The password must have at least one non-alphanumeric character, one digit, and one uppercase letter.";
        }

        public override bool IsValid(object? value)
        {
            var password = value as string;
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            if(password.Length < 6)
            {
                return false;
            }

            if (!password.Any(char.IsLetterOrDigit))
            {
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            return true;
        }
    }
}
