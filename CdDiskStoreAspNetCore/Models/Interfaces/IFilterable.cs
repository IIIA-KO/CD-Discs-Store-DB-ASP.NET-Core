namespace CdDiskStoreAspNetCore.Models.Interfaces
{
    public interface IFilterable
    {
        public string? Filter { get; set; }
        public string? FilterFieldName { get; set;}
    }
}
