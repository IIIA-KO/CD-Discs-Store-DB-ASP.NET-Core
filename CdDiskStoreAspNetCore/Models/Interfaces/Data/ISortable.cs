using CdDiskStoreAspNetCore.Models.Enums;

namespace CdDiskStoreAspNetCore.Models.Interfaces.Data
{
    public interface ISortable
    {
        public string? SortFieldName { get; set; }
        public MySortOrder SortOrder { get; set; }
    }
}