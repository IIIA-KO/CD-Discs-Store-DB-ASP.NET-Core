using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("DiscMusic")]
    public class DiscMusic
    {
        public Disc? Disc { get; set; }

        public Guid IdDisc { get; set; }

        public Music? Music { get; set; }

        public Guid IdMusic { get; set; }
    }
}
