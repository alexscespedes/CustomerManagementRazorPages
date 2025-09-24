using System;
using CustomerManagementRazorPages.Models;

namespace CustomerManagementRazorPages.Services;

public interface IOrderApiService
{
    Task<List<Order>> GetOrdersAsync();
    Task<Order?> GetOrderAsync(int id);
    Task<Order?> CreateOrderAsync(CreateOrderRequest order);
    Task<bool> DeleteOrderAsync(int id);
}
