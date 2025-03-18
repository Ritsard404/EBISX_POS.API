using EBISX_POS.API.Services.DTO.Order;

namespace EBISX_POS.API.Services.Interfaces
{
    public interface IOrder
    {
        Task<(bool, string)> AddOrderItem(AddOrderDTO addOrder);
        Task<(bool, string)> AddCurrentOrderVoid(AddCurrentOrderVoidDTO voidOrder);
        Task<(bool, string)> VoidOrderItem(VoidOrderItemDTO voidOrder);
    }
}
