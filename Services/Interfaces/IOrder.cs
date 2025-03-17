namespace EBISX_POS.API.Services.Interfaces
{
    public interface IOrder
    {
        Task<(bool, string)> AddOrderItem(int menuId, string cashierEmail);
    }
}
