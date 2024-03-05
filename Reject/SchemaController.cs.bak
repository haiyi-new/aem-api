using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MyWebApi.Models;

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemaController : ControllerBase
    {
        private readonly string _connectionString;

        public SchemaController(string connectionString)
        {
            _connectionString = connectionString;
        }

        [HttpPost("InitializeSchema")]
        public async Task<IActionResult> InitializeSchema()
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                // Create DashboardData table
                using var cmdDashboardData = new MySqlCommand(
                    "CREATE TABLE IF NOT EXISTS DashboardData (" +
                    "Id INT AUTO_INCREMENT PRIMARY KEY," +
                    "ChartName VARCHAR(255) NOT NULL," +
                    "Value DECIMAL(18, 2) NOT NULL" +
                    ")", connection);
                await cmdDashboardData.ExecuteNonQueryAsync();

                // Create Users table
                using var cmdUsers = new MySqlCommand(
                    "CREATE TABLE IF NOT EXISTS Users (" +
                    "Id INT AUTO_INCREMENT PRIMARY KEY," +
                    "FirstName VARCHAR(255) NOT NULL," +
                    "LastName VARCHAR(255) NOT NULL," +
                    "Username VARCHAR(255) NOT NULL" +
                    ")", connection);
                await cmdUsers.ExecuteNonQueryAsync();

                // Create PlatformWellActual table
                using var cmdPlatformWellActual = new MySqlCommand(
                    "CREATE TABLE IF NOT EXISTS PlatformWellActual (" +
                    "Id INT AUTO_INCREMENT PRIMARY KEY," +
                    "UniqueName VARCHAR(255) NOT NULL," +
                    "Latitude DECIMAL(18, 15) NOT NULL," +
                    "Longitude DECIMAL(18, 15) NOT NULL," +
                    "CreatedAt DATETIME NOT NULL," +
                    "UpdatedAt DATETIME NOT NULL" +
                    ")", connection);
                await cmdPlatformWellActual.ExecuteNonQueryAsync();

                // Create PlatformWellActualWell table
                using var cmdPlatformWellActualWell = new MySqlCommand(
                    "CREATE TABLE IF NOT EXISTS PlatformWellActualWell (" +
                    "Id INT AUTO_INCREMENT PRIMARY KEY," +
                    "PlatformId INT NOT NULL," +
                    "UniqueName VARCHAR(255) NOT NULL," +
                    "Latitude DECIMAL(18, 15) NOT NULL," +
                    "Longitude DECIMAL(18, 15) NOT NULL," +
                    "CreatedAt DATETIME NOT NULL," +
                    "UpdatedAt DATETIME NOT NULL," +
                    "FOREIGN KEY (PlatformId) REFERENCES PlatformWellActual(Id)" +
                    ")", connection);
                await cmdPlatformWellActualWell.ExecuteNonQueryAsync();

                // Create PlatformWellDummy table
                using var cmdPlatformWellDummy = new MySqlCommand(
                    "CREATE TABLE IF NOT EXISTS PlatformWellDummy (" +
                    "Id INT AUTO_INCREMENT PRIMARY KEY," +
                    "UniqueName VARCHAR(255) NOT NULL," +
                    "Latitude DECIMAL(18, 15) NOT NULL," +
                    "Longitude DECIMAL(18, 15) NOT NULL," +
                    "LastUpdate DATETIME NOT NULL" +
                    ")", connection);
                await cmdPlatformWellDummy.ExecuteNonQueryAsync();

                // Create PlatformWellDummyWell table
                using var cmdPlatformWellDummyWell = new MySqlCommand(
                    "CREATE TABLE IF NOT EXISTS PlatformWellDummyWell (" +
                    "Id INT AUTO_INCREMENT PRIMARY KEY," +
                    "PlatformId INT NOT NULL," +
                    "UniqueName VARCHAR(255) NOT NULL," +
                    "Latitude DECIMAL(18, 15) NOT NULL," +
                    "Longitude DECIMAL(18, 15) NOT NULL," +
                    "LastUpdate DATETIME NOT NULL," +
                    "FOREIGN KEY (PlatformId) REFERENCES PlatformWellDummy(Id)" +
                    ")", connection);
                await cmdPlatformWellDummyWell.ExecuteNonQueryAsync();

                return Ok("Database schema initialized successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to initialize database schema: {ex.Message}");
            }
        }
    }
}
