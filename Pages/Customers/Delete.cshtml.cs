using CustomerManagementRazorPages.Models;
using CustomerManagementRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManagementRazorPages.Pages.Customers
{
    public class DeleteModel : PageModel
    {
        private readonly ICustomerApiService _customerService;
        private readonly ILogger<DeleteModel> _logger;
        public DeleteModel(ICustomerApiService customerService, ILogger<DeleteModel> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;
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
                Customer = await _customerService.GetCustomerAsync(id.Value);

                if (Customer == null)
                {
                    return NotFound();
                }

                return Page();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching customer {Id} for deletion", id);
                ErrorMessage = "Unable to load customer for deletion. Please try again later.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching customer {Id} for deletion", id);
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

                var success = await _customerService.DeleteCustomerAsync(id.Value);

                if (success)
                {
                    TempData["SuccessMessage"] = $"Customer '{Customer.Name}' was deleted successfully.";
                    return RedirectToPage("./Index");
                }

                else
                {
                    ErrorMessage = "Failed to delete customer. Please try again.";
                    return Page();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error deleting customer {Id} via API", id);
                ErrorMessage = "Unable to delete customer. Please check your connection and try again.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting customer {Id}", id);
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
