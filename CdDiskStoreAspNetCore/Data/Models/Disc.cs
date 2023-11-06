using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("Disc")]
    public class Disc
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = default!;

        [Range(0.00, 99999999.99, ErrorMessage = "Price should be in the range of 0.00 to 99999999.99")]
        public decimal Price { get; set; }
    }
}
