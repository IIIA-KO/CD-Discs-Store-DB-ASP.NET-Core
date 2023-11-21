using CdDiskStoreAspNetCore.Data.Models;

namespace CdDiskStoreAspNetCore.Models
{
    public class MusicsDetailsViewModel
    {
        public Music Music { get; set; } = default!;

        public IReadOnlyList<Disc>? Discs { get; set; }
    }
}
