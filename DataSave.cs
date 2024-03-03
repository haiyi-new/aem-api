using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MyWebApi.Models;
using MyWebApi.Controllers;

namespace MyWebApi
{
    public class DataSave
    {
        private readonly string _connectionString = "Server=db;Port=3306;Database=db;Uid=user;Pwd=admin123;";

        public DataSave(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Define dataSaved variable to track if data has been saved
        private bool dataSaved = false;

        public async Task SaveData(MySqlConnection connection, SyncDataResponse dashboardData, PlatformWellActualResponse[] platformWellActualData, PlatformWellDummyResponse[] platformWellDummyData)
        {
            if (!dataSaved)
            {
                await SaveDashboardData(connection, dashboardData);
                await SavePlatformWellActualData(connection, platformWellActualData);
                await SavePlatformWellDummyData(connection, platformWellDummyData);
                dataSaved = true;
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
                await SaveDashboardChart(connection, donut?.Name ?? "Unknown", donut.Value);
            }

            foreach (var bar in dashboardData.ChartBar)
            {
                await SaveDashboardChart(connection, bar?.Name ?? "Unknown", bar.Value);
            }

            foreach (var user in dashboardData.TableUsers)
            {
                await SaveUserData(connection, user.FirstName ?? "Unknown", user.LastName ?? "Unknown", user.Username ?? "Unknown");
            }
        }

        private async Task SavePlatformWellActualData(MySqlConnection connection, PlatformWellActualResponse[] platformWellActualData)
        {
            if (platformWellActualData != null)
            {
                foreach (var platformData in platformWellActualData)
                {
                    if (platformData != null)
                    {
                        await SavePlatformActualData(connection, platformData);
                    }
                }
            }
        }

        private async Task SavePlatformWellDummyData(MySqlConnection connection, PlatformWellDummyResponse[] platformWellDummyData)
        {
            if (platformWellDummyData != null)
            {
                foreach (var platformData in platformWellDummyData)
                {
                    if (platformData != null)
                    {
                        await SavePlatformDummyData(connection, platformData);
                    }
                }
            }
        }

        private async Task SaveDashboardChart(MySqlConnection connection, string chartName, decimal value) // Change the parameter type to decimal
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

            // Ensure platformUniqueName is not null before passing it to SavePlatformActualWellData
            if (uniqueName != null)
            {
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
                    await SavePlatformActualWellData(connection, uniqueName, well);
                }
            }
        }

        private async Task SavePlatformDummyData(MySqlConnection connection, PlatformWellDummyResponse platformData)
        {
            var uniqueName = platformData.UniqueName;
            var latitude = platformData.Latitude;
            var longitude = platformData.Longitude;
            var lastUpdate = platformData.LastUpdate;

            // Ensure platformUniqueName is not null before passing it to SavePlatformDummyWellData
            if (uniqueName != null)
            {
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
