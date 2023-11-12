namespace CdDiskStoreAspNetCore.Models.Interfaces.Data
{
    public interface IFilterable
    {
        public string? Filter { get; set; }
        public string? FilterFieldName { get; set; }
    }
}
