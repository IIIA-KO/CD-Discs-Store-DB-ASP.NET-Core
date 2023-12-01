namespace CdDiskStoreAspNetCore.Data.Repository
{
    public record UselessDisc(Guid Id, string Name, decimal Price);

    public interface IUselessDiscsRepository : IGenericViewRepository<UselessDisc>
    {
    }
}
