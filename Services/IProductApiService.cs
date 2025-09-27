using System;
using CustomerManagementRazorPages.Models;

namespace CustomerManagementRazorPages.Services;

public interface IProductApiService
{
    Task<List<Product>> GetProductsAsync();
    Task<Product?> GetProductAsync(int id);
    Task<Product?> CreateProductAsync(Product product);
    Task<bool> UpdateProductAsync(int id, Product product);
    Task<bool> DeleteProductAsync(int id);
}
