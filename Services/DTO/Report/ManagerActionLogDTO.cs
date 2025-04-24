
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace EBISX_POS.API.Services.DTO.Report
{
    public class ManagerActionLogDTO
    {
        public required string Name { get; set; }
        public required string ManagerEmail { get; set; }
        public string? CashierName { get; set; }
        public string? CashierEmail { get; set; }

        public required ManagerActionType Action { get; set; }
        public DateTime ActionDate { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ManagerActionType
    {
        Login,
        Logout,
        Edit,
        Cancel,
        Refund,
        Discount,
        Void,
        [EnumMember(Value = "Set Cash In Drawer")]
        SetCashInDrawer,
        [EnumMember(Value = "Set Cash Out Drawer")]
        SetCashOutDrawer,
        [EnumMember(Value = "Cash With Draw")]
        CashWithdraw,
        ZReport,

    }
}
