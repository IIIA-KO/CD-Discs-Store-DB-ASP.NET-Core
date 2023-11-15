using CdDiskStoreAspNetCore.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace CdDiskStoreAspNetCore.Models
{
    public class ClientDetailsViewModel
    {
        public Client Client { get; set; } = default!;

        public decimal PurchaseProfit { get; set; }

        public decimal RentProfit { get; set; }

        public decimal TotalProfit { get; set; }

        public int PersonalDiscount { get; set; }
    }
}