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
                // Establish connection to the database
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                // Check if tables exist before attempting to create them
                if (await TablesExist(connection))
                {
                    return BadRequest("Database tables already exist.");
                }

                // Create DashboardData table
                await CreateDashboardDataTable(connection);

                // Create Users table
                await CreateUsersTable(connection);

                // Create PlatformWellActual table
                await CreatePlatformWellActualTable(connection);

                // Create PlatformWellActualWell table
                await CreatePlatformWellActualWellTable(connection);

                // Create PlatformWellDummy table
                await CreatePlatformWellDummyTable(connection);

                // Create PlatformWellDummyWell table
                await CreatePlatformWellDummyWellTable(connection);

                return Ok("Database schema initialized successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to initialize database schema: {ex.Message}");
            }
        }

        // Method to check if the tables already exist in the database
        private async Task<bool> TablesExist(MySqlConnection connection)
        {
            using var cmd = new MySqlCommand("SHOW TABLES", connection);
            using var reader = await cmd.ExecuteReaderAsync();
            return reader.HasRows;
        }

        // Methods to create individual tables
        private async Task CreateDashboardDataTable(MySqlConnection connection)
        {
            using var cmdDashboardData = new MySqlCommand(
                "CREATE TABLE IF NOT EXISTS DashboardData (" +
                "Id INT AUTO_INCREMENT PRIMARY KEY," +
                "ChartName VARCHAR(255) NOT NULL," +
                "Value DECIMAL(18, 2) NOT NULL" +
                ")", connection);
            await cmdDashboardData.ExecuteNonQueryAsync();
        }

        private async Task CreateUsersTable(MySqlConnection connection)
        {
            // Create Users table
            using var cmdUsers = new MySqlCommand(
                "CREATE TABLE IF NOT EXISTS Users (" +
                "Id INT AUTO_INCREMENT PRIMARY KEY," +
                "FirstName VARCHAR(255) NOT NULL," +
                "LastName VARCHAR(255) NOT NULL," +
                "Username VARCHAR(255) NOT NULL" +
                ")", connection);
            await cmdUsers.ExecuteNonQueryAsync();
        }

        private async Task CreatePlatformWellActualTable(MySqlConnection connection)
        {
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
        }

        private async Task CreatePlatformWellActualWellTable(MySqlConnection connection)
        {
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
        }

        private async Task CreatePlatformWellDummyTable(MySqlConnection connection)
        {
            using var cmdPlatformWellDummy = new MySqlCommand(
                "CREATE TABLE IF NOT EXISTS PlatformWellDummy (" +
                "Id INT AUTO_INCREMENT PRIMARY KEY," +
                "UniqueName VARCHAR(255) NOT NULL," +
                "Latitude DECIMAL(18, 15) NOT NULL," +
                "Longitude DECIMAL(18, 15) NOT NULL," +
                "LastUpdate DATETIME NOT NULL" +
                ")", connection);
            await cmdPlatformWellDummy.ExecuteNonQueryAsync();
        }

        private async Task CreatePlatformWellDummyWellTable(MySqlConnection connection)
        {
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
        }
    }
}
