using CdDiskStoreAspNetCore.Utilities.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("Client")]
    public class Client
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "The \"First name\" field is required")]
        public string FirstName { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Last name\" field is required")]
        public string LastName { get; set; } = default!;

        [StringLength(100)]
        [Required(ErrorMessage = "The \"Address\" field is required")]
        public string Address { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"City\" field is required")]
        public string City { get; set; } = default!;

        [StringLength(20)]
        [Required(ErrorMessage = "The \"Contact phone\" field is required")]
        [PhoneValidation]
        public string ContactPhone { get; set; } = default!;

        [StringLength(100)]
        [Required(ErrorMessage = "The \"Contact mail\" field is required")]
        [EmailAddressValidation]
        public string ContactMail { get; set; } = default!;

        [Required(ErrorMessage = "The \"Birth day\" field is required")]
        public DateTime BirthDay { get; set; }

        public bool MarriedStatus { get; set; } = false;

        public bool Sex { get; set; } = false;

        public bool HasChild { get; set; } = false;
    }
}