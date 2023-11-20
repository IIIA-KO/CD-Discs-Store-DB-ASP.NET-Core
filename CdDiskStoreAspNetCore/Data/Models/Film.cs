using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("Film")]
    public class Film
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Name\" field is required")]
        public string Name { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Genre\" field is required")]
        public string Genre { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Producer\" field is required")]
        public string Producer { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Main Role\" field is required")]
        public string MainRole { get; set; } = default!;

        [Range(0, 18)]
        [Required(ErrorMessage = "The \"Age Limit\" field is required")]
        public int AgeLimit { get; set; }
    }
}