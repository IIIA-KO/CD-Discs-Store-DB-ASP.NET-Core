using Microsoft.AspNetCore.Identity;

namespace CdDiskStoreAspNetCore.Models
{
    public class AdminPanelDetailsViewModel
    {
        public IdentityUser User { get; set; } = default!;

        public IReadOnlyList<string> Roles { get; set; } = default!;
    }
}
