using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyWebApi
{
    public static class ApiKeyProvider
    {
        public static string? ApiKey { get; set; }
    }

    [Route("api/[controller]")]
    public class DataSyncController : ControllerBase
    {
        private readonly string _loginUrl = "http://test-demo.aemenersol.com/api/Account/Login";
        private readonly string _dashboardUrl = "http://test-demo.aemenersol.com/api/Dashboard";
        private readonly string _platformWellActualUrl = "http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual";
        private readonly string _platformWellDummyUrl = "http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellDummy";
        private readonly string _username = "user@aemenersol.com";
        private readonly string _password = "Test@123";

        [HttpGet("SyncData")]
        public async Task<IActionResult> SyncData()
        {
            try
            {
                // Login to get the API key
                ApiKeyProvider.ApiKey = await Login();

                // Access the dashboard data using the obtained API key
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ApiKeyProvider.ApiKey);
                var response = await httpClient.GetAsync(_dashboardUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(new { message = $"Failed to access dashboard data: {response.StatusCode}" });
                }

                var responseData = await response.Content.ReadAsStringAsync();

                // Process the retrieved data (you can store it in your database)
                // Example: store data in database using _connectionString

                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error syncing data: {ex.Message}" });
            }
        }

        [HttpGet("GetPlatformWellActual")]
        public async Task<IActionResult> GetPlatformWellActual()
        {
            return await GetData(_platformWellActualUrl);
        }

        [HttpGet("GetPlatformWellDummy")]
        public async Task<IActionResult> GetPlatformWellDummy()
        {
            return await GetData(_platformWellDummyUrl);
        }

        private async Task<IActionResult> GetData(string url)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ApiKeyProvider.ApiKey);
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(new { message = $"Failed to access data: {response.StatusCode}" });
                }

                var responseData = await response.Content.ReadAsStringAsync();
                // Parse the response data as a JArray
                // Parse the response data as a JArray
                var jsonArray = JArray.Parse(responseData);

                // Iterate over the items in the jsonArray and add the contents of the well[] arrays to responseData
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    var wellArray = jsonArray[i]["well"];
                    jsonArray[i]["wellData"] = wellArray;
                }

                // Return the updated jsonArray as part of the response
                return Ok(jsonArray.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error getting data: {ex.Message}" });
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
            // Assuming the response contains the API key directly
            return responseData.Trim('"'); // Remove double quotes if present
        }
    }
}
