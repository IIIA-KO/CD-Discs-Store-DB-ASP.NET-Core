using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("DiscFilm")]
    public class DiscFilm
    {
        public Disc? Disc { get; set; }

        public Guid IdDisc { get; set; }

        public Film? Film { get; set; }

        public Guid IdFilm { get; set; }
    }
}
