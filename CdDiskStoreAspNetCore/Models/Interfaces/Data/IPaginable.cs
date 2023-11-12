namespace CdDiskStoreAspNetCore.Models.Interfaces.Data
{
    public interface IPaginable
    {
        public int Skip { get; set; }

        public int PageSize { get; set; }

        public int CountItems { get; set; }
    }
}