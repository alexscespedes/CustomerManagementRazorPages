using CustomerManagementRazorPages.Models;
using CustomerManagementRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManagementRazorPages.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly IOrderApiService _orderApiService;
        private readonly ILogger<DetailsModel> _logger;
        public DetailsModel(IOrderApiService orderApiService, ILogger<DetailsModel> logger)
        {
            _orderApiService = orderApiService;
            _logger = logger;
        }

        public Order? Order { get; set; } = default!;
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
                Order = await _orderApiService.GetOrderAsync(id.Value);

                if (Order == null)
                {
                    return NotFound();
                }

                return Page();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching order {Id} from API", id);
                ErrorMessage = "Unable to load order details. Please try again later.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching orders");
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
