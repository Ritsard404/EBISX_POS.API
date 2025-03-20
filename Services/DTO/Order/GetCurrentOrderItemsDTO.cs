namespace EBISX_POS.API.Services.DTO.Order
{
    public class GetCurrentOrderItemsDTO
    {
        // List of sub-orders (individual items in the order)
        public long EntryId { get; set; }
        public List<CurrentOrderItemsSubOrder>? SubOrders { get; set; }

        // Additional properties to display order summary (if needed)
        public int TotalQuantity => SubOrders?.FirstOrDefault()?.Quantity ?? 0;
        public decimal TotalPrice => SubOrders?.Sum(s => s.ItemSubTotal) ?? 0;
        public bool HasCurrentOrder => SubOrders != null && SubOrders.Any();
        public bool HasDiscount { get; set; } = false;
        public decimal DiscountAmount => HasDiscount ? TotalPrice * 0.20m : 0;
    }

    public class CurrentOrderItemsSubOrder
    {
        // Unique item identifiers
        public int? MenuId { get; set; }
        public int? DrinkId { get; set; }
        public int? AddOnId { get; set; }

        // Item details
        public string Name { get; set; } = string.Empty;
        public string? Size { get; set; }
        public decimal ItemPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsFirstItem { get; set; } = false;

        // ✅ Computed properties
        public decimal ItemSubTotal => ItemPrice * Quantity;

        public string DisplayName => string.IsNullOrEmpty(Size)
            ? Name + $" @{ItemPrice:F2}"
            : $"{Name} ({Size}) @{ItemPrice:F2}";

        public bool IsUpgradeMeal => ItemPrice > 0;

        public string ItemPriceString => IsFirstItem
            ? $"₱{ItemSubTotal:F2}"
            : IsUpgradeMeal
                ? $"+ ₱{ItemSubTotal:F2}"
                : "";


        // ✅ Opacity property for UI handling (optional)
        public double Opacity => IsFirstItem ? 1.0 : 0.0;
    }
}
