using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using CdDiskStoreAspNetCore.Models.Interfaces.Data;

namespace CdDiskStoreAspNetCore.Models
{
    public class DiscsIndexViewModel : IDataProcessable
    {
        public static IReadOnlyList<string> AllFieldNames { get; private set; } =
            typeof(Disc).GetProperties()
            .Select(p => p.Name)
            .ToList();

        public static IReadOnlyList<string> FilterableFieldNames { get; private set; } =
            typeof(Disc).GetProperties()
            .Where(p => p.PropertyType == typeof(string))
            .Select(p => p.Name)
            .ToList();

        public IEnumerable<Disc>? Items { get; set; }

        public string? Filter { get; set; } = default!;
        public string? FilterFieldName { get; set; } = default!;

        public string? SortFieldName { get; set; } = default!;
        public MySortOrder SortOrder { get; set; }
        
        public int Skip { get; set; }
        public int PageSize { get; set; } = 10;
        public int CountItems { get; set; }
    }
}