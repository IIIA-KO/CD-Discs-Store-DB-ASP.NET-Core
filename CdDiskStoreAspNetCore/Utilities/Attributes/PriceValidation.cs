using System.ComponentModel.DataAnnotations;

namespace CdDiskStoreAspNetCore.Utilities.Attributes
{
    public class PriceValidation : RegularExpressionAttribute
    {
        public readonly static string PricePattern = @"^\d{1,9}([.,]\d{1,2})?$";

        public PriceValidation() : base(PricePattern)
        {
            ErrorMessage = "Invalid price format";
        }
    }
}
