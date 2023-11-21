using CdDiskStoreAspNetCore.Data.Models;

namespace CdDiskStoreAspNetCore.Models
{
    public class FilmsDetailsViewModel
    {
        public Film Film { get; set; } = default!;

        public IReadOnlyList<Disc>? Discs { get; set; }
    }
}
