using System;
using System.Text;
using System.Text.Json;
using CustomerManagementRazorPages.Models;

namespace CustomerManagementRazorPages.Services;

public class ProductApiService : IProductApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ProductApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
    public async Task<Product?> CreateProductAsync(Product product)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(product, _jsonOptions);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Products", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Product>(responseJson, _jsonOptions);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"Products/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return false;
        }
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Products/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Product>(jsonString, _jsonOptions);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Products");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Product>>(jsonString, _jsonOptions)!;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return new List<Product>();
        }
    }

    public async Task<bool> UpdateProductAsync(int id, Product product)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(product, _jsonOptions);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"Products/{id}", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return false;
        }
    }
}
