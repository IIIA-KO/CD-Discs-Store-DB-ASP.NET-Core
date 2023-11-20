using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("Music")]
    public class Music
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
        [Required(ErrorMessage = "The \"Artist\" field is required")]
        public string Artist { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Language\" field is required")]
        public string Language { get; set; } = default!;
    }
}
