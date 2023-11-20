using CdDiskStoreAspNetCore.Utilities.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("Disc")]
    public class Disc
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Name\" field is required")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "The \"Price\" field is required")]
        [Range(0, 99999999.99, ErrorMessage = "Price should be in the range of 0.00 to 99999999.99")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        [PriceValidation]
        public decimal Price { get; set; }
    }
}