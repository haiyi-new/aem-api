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
        public async Task SaveData(MySqlConnection connection, SyncDataResponse dashboardData, PlatformWellActualResponse[] platformWellActualData, PlatformWellDummyResponse[] platformWellDummyData)
        {
            if (!_dataSaved)
            {
                await SaveDashboardData(connection, dashboardData);
                await SavePlatformWellActualData(connection, platformWellActualData);
                await SavePlatformWellDummyData(connection, platformWellDummyData);
                _dataSaved = true;
            }
            else
            {
                Console.WriteLine("Data has already been saved. Skipping the saving process.");
            }
        }


        public async Task SaveDashboardData(MySqlConnection connection, SyncDataResponse dashboardData)
        {
            foreach (var donut in dashboardData.ChartDonut)
            {
                await SaveDashboardChart(connection, donut.Name, donut.Value);
            }

            foreach (var bar in dashboardData.ChartBar)
            {
                await SaveDashboardChart(connection, bar.Name, bar.Value);
            }

            foreach (var user in dashboardData.TableUsers)
            {
                await SaveUser(connection, user.FirstName, user.LastName, user.Username);
            }
        }

        public async Task SavePlatformWellActualData(MySqlConnection connection, PlatformWellActualResponse[] platformWellActualData)
        {
            foreach (var platformData in platformWellActualData)
            {
                await SavePlatformActualData(connection, platformData);
            }
        }

        public async Task SavePlatformWellDummyData(MySqlConnection connection, PlatformWellDummyResponse[] platformWellDummyData)
        {
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

        private async Task SavePlatformActualWellData(MySqlConnection connection, string platformUniqueName, PlatformWellActualWell well)
        {
            var uniqueName = well.UniqueName;
            var latitude = well.Latitude;
            var longitude = well.Longitude;
            var createdAt = well.CreatedAt;
            var updatedAt = well.UpdatedAt;

            using var cmd = new MySqlCommand("INSERT INTO PlatformWellActualWell (PlatformUniqueName, UniqueName, Latitude, Longitude, CreatedAt, UpdatedAt) VALUES (@PlatformUniqueName, @UniqueName, @Latitude, @Longitude, @CreatedAt, @UpdatedAt)", connection);
            cmd.Parameters.AddWithValue("@PlatformUniqueName", platformUniqueName);
            cmd.Parameters.AddWithValue("@UniqueName", uniqueName);
            cmd.Parameters.AddWithValue("@Latitude", latitude);
            cmd.Parameters.AddWithValue("@Longitude", longitude);
            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", updatedAt);
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task SavePlatformDummyWellData(MySqlConnection connection, string platformUniqueName, PlatformWellDummyWell well)
        {
            var uniqueName = well.UniqueName;
            var latitude = well.Latitude;
            var longitude = well.Longitude;
            var lastUpdate = well.LastUpdate;

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
