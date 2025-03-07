namespace EBISX_POS.API.Services.DTO.Menu
{
    public class DrinksDTO
    {
        public int MenuId { get; set; }
        public required string MenuName { get; set; }
        public string? MenuImagePath { get; set; }
        public decimal MenuPrice { get; set; }
    }

    public class SizesWithPricesDTO
    {
        public string Size { get; set; }
        public decimal Price { get; set; }
        public List<DrinksDTO>? Drinks { get; set; }
    }
}
