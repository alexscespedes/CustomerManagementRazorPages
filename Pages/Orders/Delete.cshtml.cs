using CustomerManagementRazorPages.Models;
using CustomerManagementRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManagementRazorPages.Pages.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly IOrderApiService _orderApiService;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(IOrderApiService orderApiService, ILogger<DeleteModel> logger)
        {
            _orderApiService = orderApiService;
            _logger = logger;
        }
        [BindProperty]
        public Order Order { get; set; } = default!;
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
                Order = await _orderApiService.GetOrderAsync(id.Value);

                if (Order == null)
                {
                    return NotFound();
                }

                return Page();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching order {Id} for deletion", id);
                ErrorMessage = "Unable to load order for deletion. Please try again later.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching order {Id} for deletion", id);
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

                var success = await _orderApiService.DeleteOrderAsync(id.Value);

                if (success)
                {
                    TempData["SuccessMessage"] = $"Order '{Order.Id}' was deleted successfully.";
                    return RedirectToPage("./Index");
                }

                else
                {
                    ErrorMessage = "Failed to delete order. Please try again.";
                    return Page();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error deleting order {Id} via API", id);
                ErrorMessage = "Unable to delete order. Please check your connection and try again.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting order {Id}", id);
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
