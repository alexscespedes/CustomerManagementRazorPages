using CustomerManagementRazorPages.Models;
using CustomerManagementRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManagementRazorPages.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderApiService _orderApiService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IOrderApiService orderApiService, ILogger<IndexModel> logger)
        {
            _orderApiService = orderApiService;
            _logger = logger;
        }

        public IList<Order> Orders { get; set; } = new List<Order>();

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

                _logger.LogInformation("Fetching orders from API...");

                Orders = await _orderApiService.GetOrdersAsync();

                _logger.LogInformation($"Succesfully retrieved {Orders.Count} orders");

                if (Orders.Count == 0)
                {
                    StatusMessage = "No orders found. Add some orders to get started";
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error connectiong to Order API");
                HasError = true;
                ErrorMessage = "Unable to connect to the order service. Please try again later.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching orders");
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
