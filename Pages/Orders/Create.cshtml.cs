using System.Text.Json;
using CustomerManagementRazorPages.Models;
using CustomerManagementRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManagementRazorPages.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly ICustomerApiService _customerApiService;
        private readonly IProductApiService _productApiService;
        private readonly IOrderApiService _orderApiService;
        private readonly ILogger<CreateModel> _logger;
        public CreateModel(ICustomerApiService customerApiService, IProductApiService productApiService, IOrderApiService orderApiService, ILogger<CreateModel> logger)
        {
            _customerApiService = customerApiService;
            _productApiService = productApiService;
            _orderApiService = orderApiService;
            _logger = logger;
        }

        [BindProperty]
        public CreateOrderRequest Order { get; set; } = default!;

        public List<Customer> Customers { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsSubmitting { get; set; } = false;

        public async Task OnGetAsync()
        {
            await LoadCustomersAsync();
            await LoadProductsAsync();
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

                //reload data
                await LoadCustomersAsync();
                await LoadProductsAsync();

                var selectedProduct = Products.FirstOrDefault(p => p.Id == Order.ProductId);

                if (selectedProduct == null)
                {
                    ErrorMessage = $"Selected product {Order.ProductId} not found.";
                    return Page();
                }

                if (Order.Quantity > selectedProduct.StockQuantity)
                {
                    ErrorMessage = $"Quantity cannot exceed available stock ({selectedProduct.StockQuantity})";
                    return Page();
                }

                var selectedCustomer = Customers.FirstOrDefault(c => c.Id == Order.CustomerId);
                var subtotal = selectedProduct.Price * Order.Quantity;
                var discount = selectedCustomer?.CustomerType == Enums.CustomerType.Premium ? 0.10m : 0m;
                var TotalAmount = subtotal - (subtotal * discount);

                Order = new CreateOrderRequest
                {
                    CustomerId = Order.CustomerId,
                    ProductId = Order.ProductId,
                    Quantity = Order.Quantity,
                    TotalAmount = TotalAmount,
                    OrderDate = DateTime.Now,
                    Status = Enums.OrderStatus.Pending,
                    Notes = Order.Notes
                };

                var createdOrder = await _orderApiService.CreateOrderAsync(Order);

                if (createdOrder != null)
                {
                    TempData["SuccessMessage"] = $"Order was created successfully";
                    return RedirectToPage("./Index");
                }
                else
                {
                    ErrorMessage = "Failed to create order. Please try again.";
                    return Page();
                }

            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error creating order via API");
                ErrorMessage = "Unable to create order. Please check your connection and try again.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating orders");
                ErrorMessage = "An unexpeced error occurred. Please try again.";
                return Page();
            }
            finally
            {
                IsSubmitting = false;
            }
        }

        private async Task LoadCustomersAsync()
        {
            Customers = await _customerApiService.GetCustomersAsync();
        }
        
        private async Task LoadProductsAsync()
        {
            Products = await _productApiService.GetProductsAsync();
        }
    }
}
