using CustomerManagementRazorPages.Models;
using CustomerManagementRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManagementRazorPages.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductApiService _productService;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(IProductApiService productService, ILogger<DetailsModel> logger)
        {
            _productService = productService;
            _logger = logger;
        }
        public Product? Product { get; set; } = default!;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsLoading { get; set; } = true;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                IsLoading = true;
                Product = await _productService.GetProductAsync(id.Value);

                if (Product == null)
                {
                    return NotFound();
                }

                return Page();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching product {Id} from API", id);
                ErrorMessage = "Unable to load product details. Please try again later.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching products");
                ErrorMessage = "An unexpeced error occurred. Please try again.";
                return Page();
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
