document.addEventListener("DOMContentLoaded", function () {
  const customerSelect = document.getElementById("customerSelect");
  const productSelect = document.getElementById("productSelect");
  const quantityInput = document.getElementById("quantityInput");
  const totalAmountInput = document.getElementById("totalAmount");
  const submitBtn = document.getElementById("submitBtn");
  const customerDetails = document.getElementById("customerDetails");
  const productDetails = document.getElementById("productDetails");
  const orderSummary = document.getElementById("orderSummary");
  const quantityValidation = document.getElementById("quantityValidation");

  let currentPrice = 0;
  let currentStock = 0;
  let customerType = "Regular";

  function updateCustomerDetails() {
    const selectedOption = customerSelect.options[customerSelect.selectedIndex];
    if (selectedOption.value) {
      customerType = selectedOption.getAttribute("data-customer-type");
      document.getElementById("customerType").textContent = customerType;

      if (customerType == "Premium") {
        document.getElementById("discountInfo").textContent =
          "(10% discount applied)";
      } else {
        document.getElementById("discountInfo").textContent = "";
      }

      customerDetails.style.display = "block";
    } else {
      customerDetails.style.display = "none";
      customerType = "Regular";
    }
    calculateTotal();
  }

  function updateProductDetails() {
    const selectedOption = productSelect.options[productSelect.selectedIndex];
    if (selectedOption.value) {
      currentPrice = parseFloat(selectedOption.getAttribute("data-price"));
      currentStock = parseInt(selectedOption.getAttribute("data-stock"));

      document.getElementById("productPrice").textContent =
        currentPrice.toFixed(2);
      document.getElementById("productStock").textContent = currentStock;

      productDetails.style.display = "block";
      quantityInput.max = currentStock;
    } else {
      productDetails.style.display = "none";
      currentPrice = 0;
      currentStock = 0;
      quantityInput.max = 999;
    }
    calculateTotal();
    validateQuantity();
  }

  function validateQuantity() {
    const quantity = parseInt(quantityInput.value) || 0;

    if (quantity > currentStock && currentStock > 0) {
      quantityValidation.textContent = `Quantity cannot exceed available stock (${currentStock})`;
      quantityValidation.style.display = "block";
      return false;
    } else {
      quantityValidation.style.display = "none";
      return true;
    }
  }

  function calculateTotal() {
    const quantity = parseInt(quantityInput.value) || 0;

    if (currentPrice > 0 && quantity > 0) {
      let total = currentPrice * quantity;
      let discount = 0;

      if (customerType == "Premiun") {
        discount = total * 0.1;
        total = total - discount;
      }

      totalAmountInput.value = total.toFixed(2);

      if (discount > 0) {
        document.getElementById(
          "discountDisplay"
        ).textContent = `Discount Applied: -$${discount.toFixed(2)}`;
      } else {
        document.getElementById("discountDisplay").textContent = "";
      }

      updateOrderSummary();
    } else {
      totalAmountInput.value = "";
      document.getElementById("discountDisplay").textContent = "";
      orderSummary.style.display = "none";
    }

    validateForm();
  }

  function updateOrderSummary() {
    if (customerSelect.value && productSelect.value && quantityInput.value) {
      const customerName =
        customerSelect.options[customerSelect.selectedIndex].getAttribute(
          "data-customer-name"
        );
      const productName =
        productSelect.options[productSelect.selectedIndex].getAttribute(
          "data-name"
        );
      const quantity = quantityInput.value;
      const total = totalAmountInput.value;

      document.getElementById("summaryContent").innerHTML = `
        <strong>Customer:</strong> ${customerName}<br>
        <strong>Product:</strong> ${productName}<br>
        <strong>Quantity:</strong> ${quantity}<br>
        <strong>Total:</strong> $${total}<br>
      `;
      orderSummary.style.display = "block";
    } else {
      orderSummary.style.display = "none";
    }
  }

  function validateForm() {
    const isValid =
      customerSelect.value &&
      productSelect.value &&
      quantityInput.value &&
      parseInt(quantityInput.value) > 0 &&
      validateQuantity();

    submitBtn.disabled = !isValid;
  }

  customerSelect.addEventListener("change", updateCustomerDetails);
  productSelect.addEventListener("change", updateProductDetails);
  quantityInput.addEventListener("input", function () {
    validateQuantity();
    calculateTotal();
  });

  validateForm();
});
