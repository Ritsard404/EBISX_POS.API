using System.Collections.Generic;

namespace EBISX_POS.API.Services.DTO.Menu
{
    public class DrinkTypeWithDrinksDTO
    {
        public int DrinkTypeId { get; set; }
        public string DrinkTypeName { get; set; }
        public List<DrinksDTO> Drinks { get; set; } = new List<DrinksDTO>();
    }
}
