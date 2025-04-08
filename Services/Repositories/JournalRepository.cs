using EBISX_POS.API.Data;
using EBISX_POS.API.Models.Journal;
using EBISX_POS.API.Services.DTO.Journal;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EBISX_POS.API.Services.Repositories
{
    public class JournalRepository(JournalContext _journal, ILogger<JournalRepository> _logger) : IJournal
    {
        public async Task<(bool isSuccess, string message)> AddPwdScAccountJournal(AddPwdScAccountJournalDTO journalDTO)
        {
            if (journalDTO is null)
            {
                return (false, "Input cannot be null.");
            }

            if (journalDTO.PwdScInfo == null || !journalDTO.PwdScInfo.Any())
            {
                return (false, "PwdScInfo list cannot be empty.");
            }

            try
            {
                // Prepare a list of valid journal entries
                var journals = new List<AccountJournal>();

                foreach (var pwdOrSc in journalDTO.PwdScInfo)
                {
                    // Validate that AccountName (pwdOrSc.Name) is not null or empty
                    if (string.IsNullOrWhiteSpace(pwdOrSc.Name))
                    {
                        _logger.LogError("Invalid journal entry: AccountName is null or empty. Skipping entry with Reference {Reference}.", pwdOrSc.OscaNum);
                        continue;
                    }

                    var journal = new AccountJournal
                    {
                        EntryNo = journalDTO.OrderId,
                        EntryLineNo = 5, // Adjust as needed
                        Status = journalDTO.Status ?? "Posted",
                        AccountName = pwdOrSc.Name,
                        Reference = pwdOrSc.OscaNum,
                        EntryDate = journalDTO.EntryDate,
                        EntryName = journalDTO.IsPWD ? "PWD" : "Senior",
                        // Optionally, set other properties as needed.
                    };

                    journals.Add(journal);

                    _logger.LogInformation("Prepared AccountJournal entry: AccountName={AccountName}, Reference={Reference}, EntryDate={EntryDate}",
                        journal.AccountName, journal.Reference, journal.EntryDate);
                }

                if (!journals.Any())
                {
                    return (false, "No valid journal entries to add. Please check your input.");
                }

                // Use bulk add to optimize database operations.
                await _journal.AccountJournal.AddRangeAsync(journals);
                await _journal.SaveChangesAsync();

                return (true, "Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding account journal entries.");
                return (false, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<(bool isSuccess, string message)> UnpostPwdScAccountJournal(long orderId, string oscaNum)
        {
            var pwdOrSc = await _journal.AccountJournal
                .FirstOrDefaultAsync(x => x.Reference == oscaNum && x.EntryNo == orderId);

            if (pwdOrSc == null)
                return (false, "Not Found Pwd/Sc");

            pwdOrSc.Status = "Unposted";
            await _journal.SaveChangesAsync();

            return (true, "Success");
        }
    }
}
