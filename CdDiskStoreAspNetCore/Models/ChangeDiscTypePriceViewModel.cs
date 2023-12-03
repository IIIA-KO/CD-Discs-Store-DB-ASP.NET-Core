using CdDiskStoreAspNetCore.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CdDiskStoreAspNetCore.Models
{
    public class ChangeDiscTypePriceViewModel
    {
        [Required(ErrorMessage = "The \"DiscType\" field is required")]
        public DiscType DiscType { get; set; } = default!;

        [Required(ErrorMessage = "The \"Percent\" field is required")]
        [Range(0, 100, ErrorMessage = "Values from 0 to 100 for \"Percent\" are allowed")]
        public int Percent { get; set; }

        public bool Increase { get; set; }
    }
}