using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MyWebApi.Models;

namespace MyWebApi
{
    public class DataSave
    {
        private readonly string _connectionString;

        public DataSave(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task SaveData(SyncDataResponse dashboardData, PlatformWellActualResponse[] platformWellActualData, PlatformWellDummyResponse[] platformWellDummyData)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            // Save dashboard data
            foreach (var donut in dashboardData.ChartDonut)
            {
                await SaveDashboardData(connection, donut.Name, donut.Value);
            }

            foreach (var bar in dashboardData.ChartBar)
            {
                await SaveDashboardData(connection, bar.Name, bar.Value);
            }

            foreach (var user in dashboardData.TableUsers)
            {
                await SaveUserData(connection, user.FirstName, user.LastName, user.Username);
            }

            // Save platform well actual data
            foreach (var platformData in platformWellActualData)
            {
                await SavePlatformActualData(connection, platformData); 
            }
            // Save platform well dummy data
            foreach (var platformData in platformWellDummyData)
            {
                await SavePlatformDummyData(connection, platformData);
            }
        }


        private async Task SaveDashboardData(MySqlConnection connection, string chartName, decimal value) // Change the parameter type to decimal
        {
            using var cmd = new MySqlCommand("INSERT INTO DashboardData (ChartName, Value) VALUES (@ChartName, @Value)", connection);
            cmd.Parameters.AddWithValue("@ChartName", chartName);
            cmd.Parameters.AddWithValue("@Value", value);
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task SaveUserData(MySqlConnection connection, string firstName, string lastName, string username)
        {
            using var cmd = new MySqlCommand("INSERT INTO Users (FirstName, LastName, Username) VALUES (@FirstName, @LastName, @Username)", connection);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@Username", username);
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task SavePlatformActualData(MySqlConnection connection, PlatformWellActualResponse platformData)
        {
            var uniqueName = platformData.UniqueName;
            var latitude = platformData.Latitude;
            var longitude = platformData.Longitude;
            var createdAt = platformData.CreatedAt;
            var updatedAt = platformData.UpdatedAt;

            // Save platform data
            using var cmd = new MySqlCommand("INSERT INTO PlatformWellActual (UniqueName, Latitude, Longitude, CreatedAt, UpdatedAt) VALUES (@UniqueName, @Latitude, @Longitude, @CreatedAt, @UpdatedAt)", connection);
            cmd.Parameters.AddWithValue("@UniqueName", uniqueName);
            cmd.Parameters.AddWithValue("@Latitude", latitude);
            cmd.Parameters.AddWithValue("@Longitude", longitude);
            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", updatedAt);
            await cmd.ExecuteNonQueryAsync();

            // Save well data
            foreach (var well in platformData.Well)
            {
                await SavePlatformActualWellData(connection, platformData.UniqueName, well);
            }
        }

        private async Task SavePlatformDummyData(MySqlConnection connection, PlatformWellDummyResponse platformData)
        {
            var uniqueName = platformData.UniqueName;
            var latitude = platformData.Latitude;
            var longitude = platformData.Longitude;
            var lastUpdate = platformData.LastUpdate;

            // Save platform data
            using var cmd = new MySqlCommand("INSERT INTO PlatformWellDummy (UniqueName, Latitude, Longitude, LastUpdate) VALUES (@UniqueName, @Latitude, @Longitude, @LastUpdate)", connection);
            cmd.Parameters.AddWithValue("@UniqueName", uniqueName);
            cmd.Parameters.AddWithValue("@Latitude", latitude);
            cmd.Parameters.AddWithValue("@Longitude", longitude);
            cmd.Parameters.AddWithValue("@LastUpdate", lastUpdate);
            await cmd.ExecuteNonQueryAsync();

            // Save well data
            foreach (var well in platformData.Well)
            {
                await SavePlatformDummyWellData(connection, uniqueName, well);
            }
        }

        private async Task SavePlatformActualWellData(MySqlConnection connection, string platformUniqueName, PlatformWellActualWell wellData)
        {
            var uniqueName = wellData.UniqueName;
            var latitude = wellData.Latitude;
            var longitude = wellData.Longitude;
            var createdAt = wellData.CreatedAt;
            var updatedAt = wellData.UpdatedAt;

            using var cmd = new MySqlCommand("INSERT INTO PlatformWellActualWell (PlatformUniqueName, UniqueName, Latitude, Longitude, CreatedAt, UpdatedAt) VALUES (@PlatformUniqueName, @UniqueName, @Latitude, @Longitude, @CreatedAt, @UpdatedAt)", connection);
            cmd.Parameters.AddWithValue("@PlatformUniqueName", platformUniqueName);
            cmd.Parameters.AddWithValue("@UniqueName", uniqueName);
            cmd.Parameters.AddWithValue("@Latitude", latitude);
            cmd.Parameters.AddWithValue("@Longitude", longitude);
            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", updatedAt);
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task SavePlatformDummyWellData(MySqlConnection connection, string platformUniqueName, PlatformWellDummyWell wellData)
        {
            var uniqueName = wellData.UniqueName;
            var latitude = wellData.Latitude;
            var longitude = wellData.Longitude;
            var lastUpdate = wellData.LastUpdate;

            using var cmd = new MySqlCommand("INSERT INTO PlatformWellDummyWell (PlatformUniqueName, UniqueName, Latitude, Longitude, LastUpdate) VALUES (@PlatformUniqueName, @UniqueName, @Latitude, @Longitude, @LastUpdate)", connection);
            cmd.Parameters.AddWithValue("@PlatformUniqueName", platformUniqueName);
            cmd.Parameters.AddWithValue("@UniqueName", uniqueName);
            cmd.Parameters.AddWithValue("@Latitude", latitude);
            cmd.Parameters.AddWithValue("@Longitude", longitude);
            cmd.Parameters.AddWithValue("@LastUpdate", lastUpdate);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
