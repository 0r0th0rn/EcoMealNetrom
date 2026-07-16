using System.Net.Http.Headers;
using EcoMeal.Site.Models;

namespace EcoMeal.Site.Services;

public class OrderService
{
    private readonly HttpClient _http;
    private readonly AuthService _authService;
    public OrderService(HttpClient http, AuthService authService)
    {
        _http = http;
        _authService = authService;
    }

    public HttpClient GetHttpClient() => _http;

    public async Task<bool> PlaceOrderAsync(int packageId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/order")
        {
            Content = JsonContent.Create(new { PackageId = packageId })
        };
        await AddAuthHeaderAsync(request);

        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<OrderGetModel>> GetMyOrderAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/order");
        await AddAuthHeaderAsync(request);

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var orders = await response.Content.ReadFromJsonAsync<List<OrderGetModel>>();
        return orders ?? new List<OrderGetModel>();
    }

    public async Task<List<OrderGetModel>> GetAllOrdersAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/order/all");
        await AddAuthHeaderAsync(request);

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var orders = await response.Content.ReadFromJsonAsync<List<OrderGetModel>>();
        return orders ?? new List<OrderGetModel>();
    }

    private async Task AddAuthHeaderAsync(HttpRequestMessage request)
    {
        if (string.IsNullOrEmpty(_authService.Token))
        {
            await _authService.LoadTokenAsync();
        }

        if (!string.IsNullOrEmpty(_authService.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
        }
    }

    public async Task<bool> MarkAsPickedUpAsync(int orderId)
    {
        var response = await _http.PatchAsync($"api/order/{orderId}/pickup", null);
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> CompleteOrderAsync(int orderId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/order/{orderId}/completeaza");
            await AddAuthHeaderAsync(request);

            var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
    public async Task<UserStatsModel> GetMyStatsAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/order/stats");
            await AddAuthHeaderAsync(request);

            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserStatsModel>() ?? new UserStatsModel();
        }
        catch
        {
            return new UserStatsModel();
        }
    }
}