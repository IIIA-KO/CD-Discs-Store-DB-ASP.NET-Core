using System.ComponentModel.DataAnnotations;

namespace CdDiskStoreAspNetCore.Utilities.Attributes
{
    public class EmailAddressValidation : RegularExpressionAttribute
    {
        public readonly static string EmailPattern = @"^([a-zA-Z0–9._%-]+@[a-zA-Z0–9.-]+\.[a-zA-Z]{2,6})*$";

        public EmailAddressValidation() : base(EmailPattern)
        {
            ErrorMessage = "Invalid email format";
        }
    }
}
