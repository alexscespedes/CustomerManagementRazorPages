using CustomerManagementRazorPages.Models;
using CustomerManagementRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManagementRazorPages.Pages.Customers
{
    public class DetailsModel : PageModel
    {
        private readonly ICustomerApiService _customerService;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(ICustomerApiService customerApiService, ILogger<DetailsModel> logger)
        {
            _customerService = customerApiService;
            _logger = logger;
        }

        public Customer? Customer { get; set; } = default!;
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
    }
}
