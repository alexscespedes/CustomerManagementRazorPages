using CustomerManagementRazorPages.Models;
using CustomerManagementRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManagementRazorPages.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerApiService _customerService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ICustomerApiService customerService, ILogger<IndexModel> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        public IList<Customer> Customers { get; set; } = new List<Customer>();

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

                _logger.LogInformation("Fetching customers from API...");

                Customers = await _customerService.GetCustomersAsync();

                _logger.LogInformation($"Succesfully retrieved {Customers.Count} customers");

                if (Customers.Count == 0)
                {
                    StatusMessage = "No customers found. Add some customers to get started";
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
                _logger.LogError(ex, "Unexpected error while fetching customers");
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
