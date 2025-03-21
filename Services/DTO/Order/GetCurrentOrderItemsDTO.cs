namespace EBISX_POS.API.Services.DTO.Order
{
    public class GetCurrentOrderItemsDTO
    {
        // List of sub-orders (individual items in the order)
        public long EntryId { get; set; }
        public List<CurrentOrderItemsSubOrder>? SubOrders { get; set; }

        // Additional properties to display order summary (if needed)
        public int TotalQuantity => SubOrders?.FirstOrDefault()?.Quantity ?? 0;
        public decimal TotalPrice => SubOrders?
            .Where(i => !(i.AddOnId == null && i.MenuId == null && i.DrinkId == null))
            .Sum(s => s.ItemSubTotal) ?? 0;
        public bool HasCurrentOrder => SubOrders != null && SubOrders.Any();
        public bool HasDiscount { get; set; } = false;
        public decimal DiscountAmount => SubOrders?
            .Where(i => (i.AddOnId == null && i.MenuId == null && i.DrinkId == null))
            .Sum(s => s.ItemSubTotal) ?? 0;
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
            ? Name + $" @{ItemPrice:G29}"
            : $"{Name} ({Size}) @{ItemPrice:G29}";

        public bool IsUpgradeMeal => ItemPrice > 0;

        public string ItemPriceString => IsFirstItem ? "₱" + ItemSubTotal.ToString("G29")
            : MenuId == null && DrinkId == null && AddOnId == null ? "- ₱" + ItemSubTotal.ToString("G29")
            : IsUpgradeMeal ? "+ ₱" + ItemSubTotal.ToString("G29")
            : "";


        // ✅ Opacity property for UI handling (optional)
        public double Opacity => IsFirstItem ? 1.0 : 0.0;
    }
}
