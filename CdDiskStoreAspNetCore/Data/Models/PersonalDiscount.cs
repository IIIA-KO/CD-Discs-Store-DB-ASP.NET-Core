using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("PersonalDiscount")]
    public class PersonalDiscount
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = default!;

        public Client? Client { get; set; }
        public Guid IdClient { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int PersonalDiscountValue { get; set; }
    }
}
