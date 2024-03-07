using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyWebApi.Models
{
    public class MyDbContext : DbContext
    {
        static readonly string connectionString = "Server=db; User ID=user; Password=admin123; Database=db";

        public DbSet<PlatformWellActualResponse> PlatformWellActualResponse {get; set;}
        public DbSet<PlatformWellActualWell> PlatformWellActualWell {get; set;}
        public DbSet<PlatformWellDummyResponse> PlatformWellDummyResponse {get; set;}
        public DbSet<PlatformWellActualWell> PlatformWellDummyWell {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
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
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<PlatformWellActualWell> Well { get; set; } = new List<PlatformWellActualWell>();
    }

    // Model for the response of http://localhost/api/DataSync/GetPlatformWellDummy
    public class PlatformWellDummyResponse
    {
        public int Id { get; set; }
        public string? UniqueName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<PlatformWellDummyWell> Well { get; set; } = new List<PlatformWellDummyWell>();
    }

    // Database table model for PlatformWellActualWell
    public class PlatformWellActualWell
    {
        public int Id { get; set; }
        public int PlatformId { get; set; }
        public string? UniqueName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }


    // Database table model for PlatformWellDummyWell
    public class PlatformWellDummyWell
    {
        public int Id { get; set; }
        public int PlatformId { get; set; }
        public string? UniqueName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
