using CustomerManagementRazorPages.Models;
using CustomerManagementRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManagementRazorPages.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly ICustomerApiService _customerService;
        private readonly ILogger<EditModel> _logger;

        public EditModel(ICustomerApiService customerService, ILogger<EditModel> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [BindProperty]
        public Customer? Customer { get; set; } = default!;
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
                Customer = await _customerService.GetCustomerAsync(id.Value);

                if (Customer == null)
                {
                    return NotFound();
                }

                return Page();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching customer {Id} from API", id);
                ErrorMessage = "Unable to load customer details. Please try again later.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching customers");
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

                var success = await _customerService.UpdateCustomerAsync(Customer.Id, Customer);

                if (success)
                {
                    TempData["SuccessMessage"] = $"Customer '{Customer.Name}' was updated successfully.";
                    return RedirectToPage("./Index");
                }

                else
                {
                    ErrorMessage = "Failed to update customer. Please try again.";
                    return Page();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error updating customer via API");
                ErrorMessage = "Unable to create customer. Please check your connection and try again.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating customers");
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
