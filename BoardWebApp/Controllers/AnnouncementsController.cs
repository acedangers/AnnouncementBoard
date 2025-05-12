using BoardWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

public class AnnouncementsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AnnouncementsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("BoardApi");
        var response = await client.GetAsync("announcements");

        if (!response.IsSuccessStatusCode)
        {
            return View("Error");
        }

        var content = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<List<Announcement>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(data);
    }
}
