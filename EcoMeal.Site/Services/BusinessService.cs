using EcoMeal.Site.Models;

namespace EcoMeal.Site.Services;

public class BusinessService
{
    private readonly HttpClient _http;
    public BusinessService(HttpClient http)
    {
        _http = http;
    }
    public async Task<List<BusinessModel>> GetAllSync()
    {
        var businesses = await _http.GetFromJsonAsync<List<BusinessModel>>("api/Business");
        return businesses ?? new List<BusinessModel>();
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/Business/{id}");
        return response.IsSuccessStatusCode;
    }
}
