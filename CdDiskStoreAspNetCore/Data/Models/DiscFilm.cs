using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("DiscFilm")]
    public class DiscFilm
    {
        public Guid IdDisc { get; set; }

        public Guid IdFilm { get; set; }
    }
}
