using CdDiskStoreAspNetCore.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace CdDiskStoreAspNetCore.Models
{
    public class ClientDetailsViewModel
    {
        public Client Client { get; set; } = default!;

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal PurchaseProfit { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal RentProfit { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal TotalProfit { get; set; }

        public int PersonalDiscount { get; set; }

        public IReadOnlyList<string> ContactMails { get; set; } = default!;
        public IReadOnlyList<string> ContactPhones { get; set; } = default!;

        public bool Increase { get; set; } = false;
    }
}