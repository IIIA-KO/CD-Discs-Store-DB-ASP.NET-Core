using CdDiskStoreAspNetCore.Utilities.Attributes;
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
        [PhoneValidation]
        public string Phone { get; set; } = default!;

        public Guid IdClient { get; set; }
    }
}
