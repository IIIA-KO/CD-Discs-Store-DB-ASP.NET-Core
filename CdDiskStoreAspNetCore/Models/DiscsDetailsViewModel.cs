﻿using CdDiskStoreAspNetCore.Data.Models;

namespace CdDiskStoreAspNetCore.Models
{
    public class DiscsDetailsViewModel
    {
        public Disc Disc { get; set; } = default!;

        public string Type { get; set; } = default!;
    }
}