namespace EBISX_POS.API.Services.DTO.Menu
{
    public class AddOnTypeDTO
    {
        public int MenuId { get; set; }
        public required string MenuName { get; set; }
        public string? MenuImagePath { get; set; }
        public string? Size { get; set; }
        public decimal? Price { get; set; }
    }
}
