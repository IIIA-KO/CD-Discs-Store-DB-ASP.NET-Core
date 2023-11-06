using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("OperationType")]
    public class OperationType
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public string TypeName { get; set; } = default!;
    }
}
