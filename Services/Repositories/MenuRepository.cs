using EBISX_POS.API.Data;
using EBISX_POS.API.Models;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EBISX_POS.API.Services.Repositories
{
    public class MenuRepository(DataContext _dataContext) : IMenu
    {
        public async Task<List<Category>> Categories()
        {
            return await _dataContext.Category
                .ToListAsync();
        }

        public async Task<List<Menu>> Menus(int ctgryId)
        {
            return await _dataContext.Menu
                .Where(c => c.Category.Id == ctgryId && c.MenuIsAvailable)
                .Include(c => c.Category)
                .Include(d => d.DrinkType)
                .ToListAsync();
        }
    }
}
