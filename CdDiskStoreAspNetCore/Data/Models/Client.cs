using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("Client")]
    public class Client
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; } = default!;

        [MaxLength(50)]
        public string LastName { get; set; } = default!;

        [MaxLength(100)]
        public string Address { get; set; } = default!;

        [MaxLength(50)]
        public string City { get; set; } = default!;

        [MaxLength(15)]
        public string ContactPhone { get; set; } = default!;

        [MaxLength(100)]
        public string ContactMail { get; set; } = default!;

        public DateTime BirthDay { get; set; }

        public bool MarriedStatus { get; set; }

        public bool Sex { get; set; }

        public bool HasChild { get; set; }
    }
}