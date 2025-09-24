using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerManagementRazorPages.Models;

public class CreateOrderRequest
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required, Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
