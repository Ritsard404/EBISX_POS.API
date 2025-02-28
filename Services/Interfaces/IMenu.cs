using EBISX_POS.API.Models;

namespace EBISX_POS.API.Services.Interfaces
{
    public interface IMenu
    {
        Task<List<Category>> Categories();
        Task<List<Menu>> Menus(int ctgryId);
    }
}
