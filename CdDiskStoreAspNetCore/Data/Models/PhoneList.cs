using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("PhoneList")]
    public class PhoneList
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(20)]
        public string Phone { get; set; } = default!;

        public Client? Client { get; set; }
        public Guid IdClient { get; set; }
    }
}
