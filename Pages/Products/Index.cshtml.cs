using CustomerManagementRazorPages.Models;
using CustomerManagementRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManagementRazorPages.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductApiService _productService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IProductApiService productService, ILogger<IndexModel> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public IList<Product> Products { get; set; } = new List<Product>();

        [TempData]
        public string? StatusMessage { get; set; }

        public bool IsLoading { get; set; } = true;
        public bool HasError { get; set; } = false;
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                IsLoading = true;
                HasError = false;

                _logger.LogInformation("Fetching products from API...");

                Products = await _productService.GetProductsAsync();

                _logger.LogInformation($"Succesfully retrieved {Products.Count} products");

                if (Products.Count == 0)
                {
                    StatusMessage = "No products found. Add some products to get started";
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error connectiong to Customer API");
                HasError = true;
                ErrorMessage = "Unable to connect to the customer service. Please try again later.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching products");
                HasError = true;
                ErrorMessage = "An unexpeced error occurred. Please try again.";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
