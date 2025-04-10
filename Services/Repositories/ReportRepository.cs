using EBISX_POS.API.Data;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EBISX_POS.API.Services.Repositories
{
    public class ReportRepository(DataContext _dataContext) : IReport
    {
        public async Task<(string CashInDrawer, string CurrentCashDrawer)> CashTrack(string cashierEmail)
        {
            var timestamp = await _dataContext.Timestamp
                .Include(t => t.Cashier)
                .Where(t => t.Cashier.UserEmail == cashierEmail && t.TsOut == null && t.CashInDrawerAmount != null && t.CashInDrawerAmount >= 1000)
                .FirstOrDefaultAsync();

            if (timestamp == null || timestamp.CashInDrawerAmount == null)
                return ("₱0.00", "₱0.00");

            var tsIn = timestamp.TsIn;

            decimal totalCashInDrawer = await _dataContext.Order
                    .Where(o =>
                        o.Cashier.UserEmail == cashierEmail &&
                        !o.IsCancelled &&
                        !o.IsPending &&
                        o.CreatedAt >= tsIn &&
                        o.CashTendered != null &&
                        o.TotalAmount != 0
                    )
                    .SumAsync(o =>
                        o.CashTendered!.Value > o.TotalAmount
                            ? o.TotalAmount
                            : o.CashTendered!.Value
                    );

            var phCulture = new CultureInfo("en-PH");
            string cashInDrawerText = timestamp.CashInDrawerAmount.Value.ToString("C", phCulture);
            string currentCashDrawerText = totalCashInDrawer.ToString("C", phCulture);

            return (cashInDrawerText, currentCashDrawerText);
        }
    }
}
