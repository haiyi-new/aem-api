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
    [Route("[controller]")]
    public class DataSaveController : ControllerBase
    {
        private readonly string _loginUrl = "http://test-demo.aemenersol.com/api/Account/Login";
        private readonly string _platformWellActualUrl = "http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual";
        private readonly string _platformWellDummyUrl = "http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellDummy";
        private readonly string _username = "user@aemenersol.com";
        private readonly string _password = "Test@123";
        private string _apiKey = "";

        private readonly MyDbContext dbContext;

        public DataSaveController(MyDbContext myDbContext)
        {
            dbContext = myDbContext;
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

        private async Task<String> GetDataFromExternalApi(string url)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to access data from {url}: {response.StatusCode}");
            }

            var responseData = await response.Content.ReadAsStringAsync();

            return responseData;
        }

        [HttpGet("Actual")]
        public async Task<IActionResult> Actual()
        {
            try
            {
                _apiKey = await Login();

                if (string.IsNullOrEmpty(_apiKey))
                {
                    return BadRequest ("API key is null or empty. Failed to retrieve API key.");
                }

                var dashboardDataJsonRaw = await GetDataFromExternalApi(_platformWellActualUrl);
                var jsonObject = JsonConvert.DeserializeObject<List<PlatformWellActualResponse>>(dashboardDataJsonRaw);

                return Ok(jsonObject);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = $"Error saving data: {ex.Message}"});
            }
        }

        [HttpGet("Dummy")]
        public async Task<IActionResult> Dummy()
        {
            try
            {
                _apiKey = await Login();

                if (string.IsNullOrEmpty(_apiKey))
                {
                    return BadRequest ("API key is null or empty. Failed to retrieve API key.");
                }

                var dashboardDataJsonRaw = await GetDataFromExternalApi(_platformWellDummyUrl);
                var jsonObject = JsonConvert.DeserializeObject<List<PlatformWellDummyResponse>>(dashboardDataJsonRaw);

                return Ok(jsonObject);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = $"Error saving data: {ex.Message}"});
            }
        }
        
        [HttpGet("Actualx")]
        public async Task<IActionResult> Actualx()
        {
            _apiKey = await Login();

            if (string.IsNullOrEmpty(_apiKey))
            {
                return BadRequest("API key is null or empty. Failed to retrieve API key.");
            }

            var dashboardDataJsonRaw = await GetDataFromExternalApi(_platformWellActualUrl);
            var jsonObject = JsonConvert.DeserializeObject<List<PlatformWellActualResponse>>(dashboardDataJsonRaw);

            // create new database
            var well = new PlatformWellActualWell
            {
                Id = 126666,
                PlatformId = 13,
                UniqueName = "Test",
                Latitude = 12.0,
                Longitude = 13.0,
                CreatedAt = new DateTime(),
                UpdatedAt = new DateTime(),
            };

            dbContext.PlatformWellActualResponse.AddRange(jsonObject);
            //dbContext.PlatformWellActualWell.Add(well);

            await dbContext.SaveChangesAsync();

            return Ok(jsonObject.First().Well.First());
        }

        [HttpGet("Dummyx")]
        public async Task<IActionResult> Dummyx()
        {
            _apiKey = await Login();

            if (string.IsNullOrEmpty(_apiKey))
            {
                return BadRequest("API key is null or empty. Failed to retrieve API key.");
            }

            var dashboardDataJsonRaw = await GetDataFromExternalApi(_platformWellDummyUrl);
            var jsonObject = JsonConvert.DeserializeObject<List<PlatformWellDummyResponse>>(dashboardDataJsonRaw);

            // create new database
            var well = new PlatformWellActualWell
            {
                Id = 126667,
                PlatformId = 13,
                UniqueName = "Test2",
                Latitude = 12.0,
                Longitude = 13.0,
                CreatedAt = new DateTime(),
                UpdatedAt = new DateTime(),
            };

            dbContext.PlatformWellDummyResponse.AddRange(jsonObject);
            //dbContext.PlatformWellActualWell.Add(well);

            await dbContext.SaveChangesAsync();

            return Ok(jsonObject.First().Well.First());
        }

    }
}
