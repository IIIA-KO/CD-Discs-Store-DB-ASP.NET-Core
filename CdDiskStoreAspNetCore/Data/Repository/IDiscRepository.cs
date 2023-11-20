﻿using CdDiskStoreAspNetCore.Data.Models;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public interface IDiscRepository : IGenericRepository<Disc>
    {
        Task<string> GetTypeAsync(Guid? id);
    }
}
