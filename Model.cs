using System;
using System.Collections.Generic;

namespace MyWebApi.Models
{
    // Model for the response of http://localhost/api/DataSync/SyncData
    public class SyncDataResponse
    {
        public bool Success { get; set; }
        public List<ChartItem> ChartDonut { get; set; } = new List<ChartItem>();
        public List<ChartItem> ChartBar { get; set; } = new List<ChartItem>();
        public List<User> TableUsers { get; set; } = new List<User>();
    }

    public class ChartItem
    {
        public string? Name { get; set; }
        public decimal Value { get; set; }
    }

    public class User
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
    }

    // Model for the response of http://localhost/api/DataSync/GetPlatformWellActual
    public class PlatformWellActualResponse
    {
        public int Id { get; set; }
        public string? UniqueName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<PlatformWellActualWell> Well { get; set; } = new List<PlatformWellActualWell>();
    }

    // Model for the response of http://localhost/api/DataSync/GetPlatformWellDummy
    public class PlatformWellDummyResponse
    {
        public int Id { get; set; }
        public string? UniqueName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<PlatformWellDummyWell> Well { get; set; } = new List<PlatformWellDummyWell>();
    }

    // Database table model for PlatformWellActual
    public class PlatformWellActual
    {
        public int Id { get; set; }
        public string? UniqueName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // Database table model for PlatformWellActualWell
    public class PlatformWellActualWell
    {
        public int Id { get; set; }
        public int PlatformId { get; set; }
        public string? UniqueName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // Database table model for PlatformWellDummy
    public class PlatformWellDummy
    {
        public int Id { get; set; }
        public string? UniqueName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    // Database table model for PlatformWellDummyWell
    public class PlatformWellDummyWell
    {
        public int Id { get; set; }
        public int PlatformId { get; set; }
        public string? UniqueName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
