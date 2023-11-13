﻿using System.ComponentModel.DataAnnotations;

namespace CdDiskStoreAspNetCore.Utilities.Attributes
{
    public class PhoneValidation : RegularExpressionAttribute
    {
        public readonly static string PhonePattern = @"^(073|075|096|095|098|063|067|068)\d{7}$";
        public PhoneValidation() : base(PhonePattern)
        {
            ErrorMessage = "Invalid phone format";
        }
    }
}