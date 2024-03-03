using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MyWebApi.Models;
using MySql.Data.MySqlClient; // Add this namespace for MySqlConnection

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    public class DataSaveController : ControllerBase
    {
        private readonly string _loginUrl = "http://test-demo.aemenersol.com/api/Account/Login";
        private readonly string _dashboardUrl = "http://test-demo.aemenersol.com/api/Dashboard";
        private readonly string _platformWellActualUrl = "http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual";
        private readonly string _platformWellDummyUrl = "http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellDummy";
        private readonly string _username = "user@aemenersol.com";
        private readonly string _password = "Test@123";
        private string _apiKey = "";
        private readonly string _connectionString = "Server=db;Port=3306;Database=db;Uid=user;Pwd=admin123;";


        [HttpGet("SaveData")]
        public async Task<IActionResult> SaveData()
        {
            try
            {
                _apiKey = await Login();

                if (string.IsNullOrEmpty(_apiKey))
                {
                    return BadRequest("API key is null or empty. Failed to retrieve API key.");
                }

                var dashboardDataJson = await GetDataFromExternalApi(_dashboardUrl);
                var platformWellActualDataJson = await GetDataFromExternalApi(_platformWellActualUrl);
                var platformWellDummyDataJson = await GetDataFromExternalApi(_platformWellDummyUrl);

                var dashboardData = JsonConvert.DeserializeObject<SyncDataResponse>(dashboardDataJson.ToString());
                var platformWellActualData = JsonConvert.DeserializeObject<PlatformWellActualResponse[]>(platformWellActualDataJson.ToString());
                var platformWellDummyData = JsonConvert.DeserializeObject<PlatformWellDummyResponse[]>(platformWellDummyDataJson.ToString());


                // Pass the deserialized data to DataSave class for saving
                var dataSaver = new DataSave(_connectionString);
                // Define MySqlConnection object and open the connection
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    // Call the SaveData method of DataSave class with the MySqlConnection object
                    await dataSaver.SaveData(connection, dashboardData, platformWellActualData, platformWellDummyData);
                }

                // Return appropriate response
                return Ok("Data saved successfully.");
            }
            catch (Exception ex)
            {
                // Return error response
                return BadRequest(new { message = $"Error saving data: {ex.Message}" });
            }
        }

        private async Task<string> Login()
        {
            using var httpClient = new HttpClient();
            var loginData = new { username = _username, password = _password };
            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(_loginUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to login: {response.StatusCode}");
            }

            var responseData = await response.Content.ReadAsStringAsync();
            return responseData.Trim('"');
        }

        private async Task<JArray> GetDataFromExternalApi(string url)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to access data from {url}: {response.StatusCode}");
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var jsonArray = JArray.Parse(responseData);

            // Iterate over the items in the jsonArray and add the contents of the well[] arrays to responseData
            for (int i = 0; i < jsonArray.Count; i++)
            {
                var wellArray = jsonArray[i]["well"];
                jsonArray[i]["wellData"] = wellArray;
            }

            // Return the updated jsonArray
            return jsonArray;
        }
    }
}
