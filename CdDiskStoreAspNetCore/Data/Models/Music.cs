using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("Music")]
    public class Music
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = default!;

        [MaxLength(50)]
        public string Genre { get; set; } = default!;

        [MaxLength(50)]
        public string Artist { get; set; } = default!;

        [MaxLength(50)]
        public string Language { get; set; } = default!;
    }
}
