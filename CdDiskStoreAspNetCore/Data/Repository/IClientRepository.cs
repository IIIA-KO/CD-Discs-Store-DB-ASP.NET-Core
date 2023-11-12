﻿using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Models.Enums;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        bool IsClientChanged(Client currentClient, Client client);

        Task<int> CountAsync();

        Task<IReadOnlyList<Client>> GetProcessedData(string? filter, string? filterField, MySortOrder sortOrder, string? sortField, int skip, int pageSize);
    }
}
