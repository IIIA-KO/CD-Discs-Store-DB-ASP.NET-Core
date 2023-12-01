namespace CdDiskStoreAspNetCore.Data.Repository
{
    public record Debtor(Guid Id, string FullName, DateTime InDebtFrom, DateTime To);

    public interface IDebtorsRepository : IGenericViewRepository<Debtor>
    {

    }
}