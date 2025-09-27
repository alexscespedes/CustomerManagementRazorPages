using System;
using System.ComponentModel.DataAnnotations;
using CustomerManagementRazorPages.Enums;

namespace CustomerManagementRazorPages.Models;

public class CreateOrderRequest
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required, Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required, Range(0, int.MaxValue)]
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; }
    public string? Notes { get; set; }
    
}
