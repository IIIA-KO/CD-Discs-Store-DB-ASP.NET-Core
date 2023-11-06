using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("Film")]
    public class Film
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = default!;

        [MaxLength(50)]
        public string Genre { get; set; } = default!;

        [MaxLength(50)]
        public string Producer { get; set; } = default!;

        [MaxLength(50)]
        public string MainRole { get; set; } = default!;

        [Range(0, 18)]
        public int AgeLimit { get; set; }
    }
}