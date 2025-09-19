using CustomerManagementRazorPages.Models;
using CustomerManagementRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManagementRazorPages.Pages.Customers
{
    public class CreateModel : PageModel
    {
        private readonly ICustomerApiService _customerService;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ICustomerApiService customerService, ILogger<CreateModel> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsSubmitting { get; set; } = false;
        public IActionResult OnGet()
        {
            Customer = new Customer
            {
                CustomerType = Enums.CustomerType.Regular,
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

                Customer.CreatedDate = DateTime.Now;
                Customer.IsActive = true;

                var createdCustomer = await _customerService.CreateCustomerAsync(Customer);

                if (createdCustomer != null)
                {
                    TempData["SuccessMessage"] = $"Customer '{Customer.Name}' was created successfully";
                    return RedirectToPage("./Index");
                }
                else
                {
                    ErrorMessage = "Failed to create customer. Please try again.";
                    return Page();

                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error creating customer via API");
                ErrorMessage = "Unable to create customer. Please check your connection and try again.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating customers");
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
