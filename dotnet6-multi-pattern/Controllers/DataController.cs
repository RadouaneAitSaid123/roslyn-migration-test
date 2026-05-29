using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace Dotnet6MultiPattern.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private static readonly HttpClient _httpClient = new HttpClient();

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var sqlQuery = "SELECT u.id, u.name, u.email " +
                       "FROM users u " +
                       "INNER JOIN orders o ON o.user_id = u.id " +
                       "WHERE u.active = 1 " +
                       "ORDER BY u.created_at DESC";

        var response = await _httpClient.GetAsync("https://api.example.com/users");
        var content = await response.Content.ReadAsStringAsync();

        var data = JsonConvert.DeserializeObject<List<UserDto>>(content);
        var json = JsonConvert.SerializeObject(data);

        return Ok(new { Query = sqlQuery, Data = json });
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserDto user)
    {
        var payload = JsonConvert.SerializeObject(user);
        var httpContent = new StringContent(payload);
        var response = await _httpClient.PostAsync("https://api.example.com/users", httpContent);

        return Ok();
    }
}

public class UserDto
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public int Age { get; set; }
}
