using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("OperationType")]
    public class OperationType
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(20)]
        public string TypeName { get; set; } = default!;
    }
}
