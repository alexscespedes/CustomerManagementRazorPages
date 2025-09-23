using CustomerManagementRazorPages.Models;
using CustomerManagementRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManagementRazorPages.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductApiService _productService;
        private readonly ILogger<EditModel> _logger;

        public EditModel(IProductApiService productService, ILogger<EditModel> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [BindProperty]
        public Product? Product { get; set; } = default!;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsLoading { get; set; } = true;
        public bool IsSubmitting { get; set; } = false;

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                IsSubmitting = true;

                var success = await _productService.UpdateProductAsync(Product.Id, Product);

                if (success)
                {
                    TempData["SuccessMessage"] = $"Product '{Product.Name}' was updated successfully.";
                    return RedirectToPage("./Index");
                }

                else
                {
                    ErrorMessage = "Failed to update product. Please try again.";
                    return Page();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error updating product via API");
                ErrorMessage = "Unable to create product. Please check your connection and try again.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating products");
                ErrorMessage = "An unexpeced error occurred. Please try again.";
                return Page();
            }
            finally
            {
                IsSubmitting = false;
            }
        }
    }
}
