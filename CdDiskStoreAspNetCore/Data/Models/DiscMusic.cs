using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("DiscMusic")]
    public class DiscMusic
    {
        public Guid IdDisc { get; set; }

        public Guid IdMusic { get; set; }
    }
}
