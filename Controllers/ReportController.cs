using EBISX_POS.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBISX_POS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController(IReport _report) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> CashTrack(string cashierEmail)
        {
            var (CashInDrawer, CurrentCashDrawer) = await _report.CashTrack(cashierEmail);

            return Ok(new
            {
                CashInDrawer,
                CurrentCashDrawer
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoicesByDate(DateTime dateTime)
        {
            return Ok(await _report.GetInvoicesByDate(dateTime));
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoiceById(long invId)
        {
            return Ok(await _report.GetInvoiceById(invId));
        }
    }
}
