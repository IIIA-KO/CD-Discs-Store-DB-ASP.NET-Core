using System.ComponentModel.DataAnnotations;

namespace CdDiskStoreAspNetCore.Utilities.Attributes
{
    public class PhoneValidation : RegularExpressionAttribute
    {
        public readonly static string PhonePattern = @"^\d{2}-\d{3}-\d{2}-\d{2}$";
        public PhoneValidation() : base(PhonePattern)
        {
            ErrorMessage = "Invalid phone format";
        }
    }
}