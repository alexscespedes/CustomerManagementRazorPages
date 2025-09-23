using CustomerManagementRazorPages.Models;
using CustomerManagementRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManagementRazorPages.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IProductApiService _productService;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(IProductApiService productService, ILogger<CreateModel> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsSubmitting { get; set; } = false;

        public IActionResult OnGet()
        {
            Product = new Product
            {
                CreatedDate = DateTime.Now,
                IsActive = true
            };
            return Page();
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

                Product.CreatedDate = DateTime.Now;
                Product.IsActive = true;

                var createdProduct = await _productService.CreateProductAsync(Product);

                if (createdProduct != null)
                {
                    TempData["SuccessMessage"] = $"Product '{Product.Name}' was created successfully";
                    return RedirectToPage("./Index");
                }
                else
                {
                    ErrorMessage = "Failed to create product. Please try again.";
                    return Page();

                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error creating product via API");
                ErrorMessage = "Unable to create product. Please check your connection and try again.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating products");
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
