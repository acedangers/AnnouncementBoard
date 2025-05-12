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

    public async Task<IActionResult> Index(string? category, string? subcategory)
    {
        var client = _httpClientFactory.CreateClient("BoardApi");
        var response = await client.GetAsync("announcements");

        if (!response.IsSuccessStatusCode)
            return View(new List<Announcement>());

        var content = await response.Content.ReadAsStringAsync();
        var allAnnouncements = JsonSerializer.Deserialize<List<Announcement>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Data for category filter — from the full list of announcements.
        ViewBag.Categories = allAnnouncements.Select(a => a.Category).Distinct().ToList();
        ViewBag.SelectedCategory = category;

        // Category filtered list.
        var filteredCategory = allAnnouncements;

        if (!string.IsNullOrEmpty(category))
            filteredCategory = filteredCategory.Where(a => a.Category == category).ToList();

        // Data for subcategory filter — from the filtered list by category.
        ViewBag.Subcategories = filteredCategory.Select(a => a.SubCategory).Distinct().ToList();
        ViewBag.SelectedSubcategory = subcategory;

        // SubCategory filtered list.
        var filteredSubCategory = filteredCategory;

        if (!string.IsNullOrEmpty(subcategory))
            filteredSubCategory = filteredSubCategory.Where(a => a.SubCategory == subcategory).ToList();

        return View(filteredSubCategory);
    }


    // GET: Announcements/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Announcements/Create
    [HttpPost]
    public async Task<IActionResult> Create(Announcement announcement)
    {
        if (!ModelState.IsValid)
            return View(announcement);

        var client = _httpClientFactory.CreateClient("BoardApi");

        var json = JsonSerializer.Serialize(announcement);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("announcements", content);

        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, "Помилка при створенні оголошення.");
        return View(announcement);
    }

    // GET: Announcements/Edit/{id}
    public async Task<IActionResult> Edit(int id)
    {
        var client = _httpClientFactory.CreateClient("BoardApi");
        var response = await client.GetAsync($"announcements/{id}");

        if (!response.IsSuccessStatusCode)
            return NotFound();

        var content = await response.Content.ReadAsStringAsync();
        var announcement = JsonSerializer.Deserialize<Announcement>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(announcement);
    }

    // POST: Announcements/Edit/{id}
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Announcement announcement)
    {
        if (id != announcement.Id || !ModelState.IsValid)
            return View(announcement);

        var client = _httpClientFactory.CreateClient("BoardApi");

        var json = JsonSerializer.Serialize(announcement);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"announcements/{id}", content);

        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, "Помилка при оновленні оголошення.");
        return View(announcement);
    }

    // GET: Announcements/Delete/{id}
    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("BoardApi");
        var response = await client.GetAsync($"announcements/{id}");

        if (!response.IsSuccessStatusCode)
            return NotFound();

        var content = await response.Content.ReadAsStringAsync();
        var announcement = JsonSerializer.Deserialize<Announcement>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(announcement);
    }

    // POST: Announcements/Delete/{id}
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var client = _httpClientFactory.CreateClient("BoardApi");
        var response = await client.DeleteAsync($"announcements/{id}");

        return RedirectToAction(nameof(Index));
    }


}
