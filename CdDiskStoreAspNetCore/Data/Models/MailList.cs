using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("MailList")]
    public class MailList
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Mail { get; set; } = default!;

        public Client? Client { get; set; }
        public Guid IdClient { get; set; }
    }
}