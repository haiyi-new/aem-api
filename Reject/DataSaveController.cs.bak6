using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

                // Process the retrieved data as needed (e.g., save to database)
                // Example: var dashboardData = dashboardDataJson.ToObject<YourDashboardModel>();

                return Ok("Data saved successfully.");
            }
            catch (Exception ex)
            {
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
