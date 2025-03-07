using EBISX_POS.API.Data;
using EBISX_POS.API.Models;
using EBISX_POS.API.Services.DTO.Menu;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EBISX_POS.API.Services.Repositories
{
    public class MenuRepository(DataContext _dataContext) : IMenu
    {
        public async Task<List<AddOnTypeWithAddOnsDTO>> AddOns(int menuId)
        {
            var menuExist = await _dataContext.Menu
                .FirstOrDefaultAsync(m => m.Id == menuId && m.MenuIsAvailable && m.HasAddOn);

            // If no add-ons found, return empty lists
            if (menuExist == null)
            {
                return (new List<AddOnTypeWithAddOnsDTO>());
            }

            // Get add-ons for the given menuId and group by AddOnType
            var addOns = await _dataContext.Menu
                .Where(m => m.MenuIsAvailable && m.IsAddOn && m.AddOnType != null)
                .Select(m => new
                {
                    m.AddOnType!.Id,
                    m.AddOnType.AddOnTypeName,
                    AddOn = new AddOnTypeDTO
                    {
                        MenuId = m.Id,
                        MenuName = m.MenuName,
                        MenuImagePath = m.MenuImagePath,
                        Price = m.MenuPrice,
                        Size = m.Size
                    }
                })
                .ToListAsync();

            var groupedAddOns = addOns
                .GroupBy(a => new { a.Id, a.AddOnTypeName })
                .Select(g => new AddOnTypeWithAddOnsDTO
                {
                    AddOnTypeId = g.Key.Id,
                    AddOnTypeName = g.Key.AddOnTypeName,
                    AddOns = g
                        .Select(a => a.AddOn)
                        .ToList()
                })
                .ToList();

            return (groupedAddOns);
        }

        public async Task<List<Category>> Categories()
        {
            return await _dataContext.Category
                .ToListAsync();
        }

        public async Task<(List<DrinkTypeWithDrinksDTO>, List<string>)> Drinks(int menuId)
        {
            var menuExists = await _dataContext.Menu
                .FirstOrDefaultAsync(m => m.Id == menuId && m.MenuIsAvailable && m.HasDrink);

            if (menuExists == null)
            {
                return (new List<DrinkTypeWithDrinksDTO>(), new List<string>());
            }

            // Get menus for available drinks, including Size and Price.
            var queryResults = await _dataContext.Menu
                .Where(m => m.MenuIsAvailable && m.DrinkType != null)
                .Select(m => new
                {
                    DrinkTypeId = m.DrinkType.Id,
                    DrinkTypeName = m.DrinkType.DrinkTypeName,
                    Drink = new DrinksDTO
                    {
                        MenuId = m.Id,
                        MenuName = m.MenuName,
                        MenuImagePath = m.MenuImagePath,
                        MenuPrice = m.MenuPrice
                    },
                    Size = m.Size,
                    Price = m.MenuPrice
                })
                .ToListAsync();

            // Group by DrinkType and project to our DTO.
            var groupedDrinks = queryResults
                .GroupBy(d => new { d.DrinkTypeId, d.DrinkTypeName })
                .Select(g => new DrinkTypeWithDrinksDTO
                {
                    DrinkTypeId = g.Key.DrinkTypeId,
                    DrinkTypeName = g.Key.DrinkTypeName,
                    // For the list of drinks, group by drink name so duplicates are removed.
                    //Drinks = g.GroupBy(x => x.Drink.MenuName)
                    //          .Select(dg => dg.First().Drink)
                    //          .ToList(),
                    // Group by size to create SizesWithPrices list.
                    SizesWithPrices = g
                        .Where(x => !string.IsNullOrEmpty(x.Size))
                        .GroupBy(x => x.Size)
                        .Select(sizeGroup => new SizesWithPricesDTO
                        {
                            Size = sizeGroup.Key,
                            Price = sizeGroup.First().Price, // representative price for that size
                            Drinks = sizeGroup.Select(x => x.Drink)
                                              .Distinct() // assuming DrinksDTO implements equality (or adjust as needed)
                                              .ToList()
                        })
                        .ToList()
                })
                .ToList();

            // Get distinct drink sizes (safe to use Distinct since it's a simple string)
            var sizes = await _dataContext.Menu
                .Where(d => d.DrinkType != null && d.MenuIsAvailable && !string.IsNullOrEmpty(d.Size))
                .Select(d => d.Size!)
                .Distinct()
                .ToListAsync();

            return (groupedDrinks, sizes);
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
