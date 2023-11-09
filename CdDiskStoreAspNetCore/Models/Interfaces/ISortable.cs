using CdDiskStoreAspNetCore.Models.Enums;

namespace CdDiskStoreAspNetCore.Models.Interfaces
{
    public interface ISortable
    {
        public string? SortFieldName { get; set; }
        public MySortOrder SortOrder { get; set; }
    }
}