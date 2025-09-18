using System;
using System.Text;
using System.Text.Json;
using CustomerManagementRazorPages.Models;

namespace CustomerManagementRazorPages.Services;

public class CustomerApiService : ICustomerApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public CustomerApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
    public async Task<Customer?> CreateCustomerAsync(Customer customer)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(customer, _jsonOptions);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Customers", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Customer>(responseJson, _jsonOptions);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"Customers/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return false;
        }
    }

    public async Task<Customer?> GetCustomerAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Customers/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Customer>(jsonString, _jsonOptions);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Customer>> GetCustomersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Customers");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Customer>>(jsonString, _jsonOptions)!;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return new List<Customer>();
        }
    }

    public async Task<bool> UpdateCustomerAsync(int id, Customer customer)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(customer, _jsonOptions);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"Customers/{id}", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return false;
        }
    }
}
