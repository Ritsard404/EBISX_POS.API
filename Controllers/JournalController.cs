using EBISX_POS.API.Services.DTO.Journal;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBISX_POS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JournalController(IJournal _journal) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> AddPwdScAccountJournal(AddPwdScAccountJournalDTO journalDTO)
        {
            var (success, message) = await _journal.AddPwdScAccountJournal(journalDTO);
            if (success)
            {
                return Ok(message);
            }
            return BadRequest(message);
        }
    }
}
