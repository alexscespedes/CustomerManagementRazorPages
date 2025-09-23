using CustomerManagementRazorPages.Models;
using CustomerManagementRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManagementRazorPages.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IProductApiService _productService;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(IProductApiService productService, ILogger<DeleteModel> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsLoading { get; set; } = true;
        public bool IsDeleting { get; set; } = false;

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
                _logger.LogError(ex, "Error fetching product {Id} for deletion", id);
                ErrorMessage = "Unable to load product for deletion. Please try again later.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching product {Id} for deletion", id);
                ErrorMessage = "An unexpeced error occurred. Please try again.";
                return Page();
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return Page();
            }

            try
            {
                IsDeleting = true;

                var success = await _productService.DeleteProductAsync(id.Value);

                if (success)
                {
                    TempData["SuccessMessage"] = $"Product '{Product.Name}' was deleted successfully.";
                    return RedirectToPage("./Index");
                }

                else
                {
                    ErrorMessage = "Failed to delete product. Please try again.";
                    return Page();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error deleting product {Id} via API", id);
                ErrorMessage = "Unable to delete product. Please check your connection and try again.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting product {Id}", id);
                ErrorMessage = "An unexpeced error occurred. Please try again.";
                return Page();
            }
            finally
            {
                IsDeleting = false;
            }
        }
    }
}
