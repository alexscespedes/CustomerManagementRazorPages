using System;
using System.Text;
using System.Text.Json;
using CustomerManagementRazorPages.Models;

namespace CustomerManagementRazorPages.Services;

public class OrderApiService : IOrderApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public OrderApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }


    public async Task<Order?> CreateOrderAsync(CreateOrderRequest order)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(order, _jsonOptions);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Orders", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Order>(responseJson, _jsonOptions);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return null;
        }
    }
    

    public async Task<bool> DeleteOrderAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"Orders/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return false;
        }
    }

    public async Task<Order?> GetOrderAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Orders/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Order>(jsonString, _jsonOptions);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Orders");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Order>>(jsonString, _jsonOptions)!;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return new List<Order>();
        }
    }
}
