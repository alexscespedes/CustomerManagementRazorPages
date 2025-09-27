using System;
using CustomerManagementRazorPages.Models;

namespace CustomerManagementRazorPages.Services;

public interface ICustomerApiService
{
    Task<List<Customer>> GetCustomersAsync();
    Task<Customer?> GetCustomerAsync(int id);
    Task<Customer?> CreateCustomerAsync(Customer customer);
    Task<bool> UpdateCustomerAsync(int id, Customer customer);
    Task<bool> DeleteCustomerAsync(int id);
}
