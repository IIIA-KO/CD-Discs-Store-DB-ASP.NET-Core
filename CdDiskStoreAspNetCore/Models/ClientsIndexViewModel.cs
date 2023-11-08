using CdDiskStoreAspNetCore.Data.Models;

namespace CdDiskStoreAspNetCore.Models
{
    public class ClientsIndexViewModel
    {
        public string FieldName { get; set; } = "FirstName";

        public string? Filter { get; set; } = default!;

        public static IReadOnlyList<string> AvailableFields { get; set; } = typeof(Client).GetProperties().Where(p => p.PropertyType == typeof(string)).Select(s => s.Name).ToList();

        public IEnumerable<Client>? Clients { get; set; }
    }
}
