using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using CdDiskStoreAspNetCore.Models.Interfaces;

namespace CdDiskStoreAspNetCore.Models
{
    public class ClientsIndexViewModel : IFilterable, ISortable
    {
        public IEnumerable<Client>? Clients { get; set; }
        public static IReadOnlyList<string> AvailableFilterFieldNames { get; set; } = typeof(Client).GetProperties()
            .Where(p => p.PropertyType == typeof(string))
            .Select(s => s.Name)
            .ToList();
        public static IReadOnlyList<string> NotAvailableFilterFieldNames { get; set; } = typeof(Client).GetProperties()
            .Where(c => c.PropertyType != typeof(string))
            .Select(s => s.Name)
            .ToList();

        public string? FilterFieldName { get; set; } = default!;
        public string? Filter { get; set; } = default!;

        public string? SortFieldName { get; set; } = default!;
        public MySortOrder SortOrder { get; set; }
    }
}