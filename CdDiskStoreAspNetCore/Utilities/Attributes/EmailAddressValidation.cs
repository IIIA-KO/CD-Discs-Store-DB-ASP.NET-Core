using System.ComponentModel.DataAnnotations;

namespace CdDiskStoreAspNetCore.Utilities.Attributes
{
    public class EmailAddressValidation : RegularExpressionAttribute
    {
        public readonly static string EmailPattern = @"^([a-zA-Zа-яА-ЯіІїЇєЄ0–9._%-]+@[a-zA-Zа-яА-ЯіІїЇєЄ0–9.-]+\.[a-zA-Zа-яА-ЯіІїЇєЄ]{2,6})*$";

        public EmailAddressValidation() : base(EmailPattern)
        {
            ErrorMessage = "Invalid email format";
        }
    }
}
