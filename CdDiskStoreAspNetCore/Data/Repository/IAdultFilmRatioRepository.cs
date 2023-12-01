namespace CdDiskStoreAspNetCore.Data.Repository
{
    public record AdultFilmRatio(
            int OrderYear,
            char Sex,
            string January,
            string February,
            string March,
            string April,
            string May,
            string June,
            string July,
            string August,
            string September,
            string October,
            string November,
            string December
        );

    public interface IAdultFilmRatioRepository : IGenericViewRepository<AdultFilmRatio>
    {
    }
}
