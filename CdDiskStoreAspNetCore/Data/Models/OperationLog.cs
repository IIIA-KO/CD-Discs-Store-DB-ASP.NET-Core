using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace CdDiskStoreAspNetCore.Data.Models
{
    [Table("OperationLog")]
    public class OperationLog
    {
        [Key]
        public Guid Id { get; set; }

        public Guid OperationTypeId { get; set; }

        public DateTime OperationDateTimeStart { get; set; }

        public DateTime OperationDateTimeEnd { get; set; }

        public Guid IdClient { get; set; }

        public Guid IdDisc { get; set; }
    }
}