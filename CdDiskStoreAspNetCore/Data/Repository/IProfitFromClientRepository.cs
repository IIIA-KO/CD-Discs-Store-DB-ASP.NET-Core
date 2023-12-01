namespace CdDiskStoreAspNetCore.Data.Repository
{
    public record ProfitFromClient(Guid ClientId, string ClientName, int? OrderYear, decimal? MusicPurchase, decimal? MusicRent, decimal? FilmPurchase, decimal? FilmRent);

    public interface IProfitFromClientRepository : IGenericViewRepository<ProfitFromClient>
    {
    }
}
