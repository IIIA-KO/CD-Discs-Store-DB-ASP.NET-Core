using System.ComponentModel.DataAnnotations;

namespace CdDiskStoreAspNetCore.Models
{
    public class ChangeDiscountLevelViewModel
    {
        [Required]
        public Guid IdClient { get; set; }

        [Required(ErrorMessage = "The \"Increase\" field is required")]
        public bool Increase { get; set; }
    }
}