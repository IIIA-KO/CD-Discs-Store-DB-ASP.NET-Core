using CdDiskStoreAspNetCore.Utilities.Attributes;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CdDiskStoreAspNetCore.Models
{
    public class AdminPanelCreateViewModel
    {
        public IdentityUser User { get; set; } = default!;

        public IReadOnlyList<string> Roles { get; set; } = default!;

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The password must be at least 6 and at max 100 characters long.", MinimumLength = 6)]
        [PasswordValidation]
        public string Password { get; set; } = default!;

        [Required]
        [PhoneValidation]
        public string PhoneNumber { get; set; } = default!;

        [Required]
        [EmailAddressValidation]
        public string Email { get; set; } = default!;
    }
}