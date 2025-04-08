using EBISX_POS.API.Data;
using EBISX_POS.API.Models;
using EBISX_POS.API.Models.Journal;
using EBISX_POS.API.Services.DTO.Journal;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EBISX_POS.API.Services.Repositories
{
    public class JournalRepository(JournalContext _journal, DataContext _dataContext, ILogger<JournalRepository> _logger) : IJournal
    {
        //public async Task<(bool isSuccess, string message)> AddItemsJournal(long orderId)
        //{

        //    var items = await _dataContext.Order
        //        .Include(o => o.Items)
        //        .Include(c => c.Coupon)
        //        .Where(o => o.Id == orderId)
        //        .SelectMany(o => o.Items)
        //        .Include(i => i.Menu)
        //        .Include(i => i.Drink)
        //        .Include(i => i.AddOn)
        //        .Include(i => i.Order)
        //        .Include(i => i.Meal)
        //        .ToListAsync();

        //    if (items.IsNullOrEmpty())
        //        return (false, "Order not found");

        //    var journals = new List<AccountJournal>();

        //    foreach (var item in items)
        //    {
        //        var journal = new AccountJournal
        //        {
        //            EntryNo = item.Order.Id,
        //            EntryLineNo = 3, // Adjust as 
        //            Status = item.IsVoid ? "Unposted" : "Posted",
        //            EntryName = item.EntryId,
        //            AccountName = item.Menu?.MenuName ?? item.Drink?.MenuName ?? item.AddOn?.MenuName ?? "Unknown",
        //            EntryDate = item.createdAt.DateTime,
        //            Description = item.Menu?.MenuName != null ? "Menu"
        //            : item.Drink?.MenuName != null ? "Drink"
        //            : "Add-On",
        //            QtyOut = item.ItemQTY,
        //            Price = Convert.ToDouble(item.ItemPrice),

        //            // Optionally, set other properties as needed.
        //        };
        //    }


        //    return (true, "Success");
        //}
        public async Task<(bool isSuccess, string message)> AddItemsJournal(long orderId)
        {
            try
            {
                _logger.LogInformation("Starting AddItemsJournal for OrderId: {OrderId}", orderId);

                var order = await _dataContext.Order
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Menu)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Drink)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.AddOn)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Meal)
                    .Include(o => o.Coupon)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found.", orderId);
                    return (false, "Order not found");
                }

                if (order.Items == null || !order.Items.Any())
                {
                    _logger.LogWarning("Order {OrderId} has no items.", orderId);
                    return (false, "No items found in the order");
                }

                var journals = new List<AccountJournal>();

                foreach (var item in order.Items)
                {
                    var accountName = item.Menu?.MenuName ?? item.Drink?.MenuName ?? item.AddOn?.MenuName ?? "Unknown";
                    var description = item.Menu != null ? "Menu"
                                     : item.Drink != null ? "Drink"
                                     : item.AddOn != null ? "Add-On"
                                     : "Unknown";

                    var journal = new AccountJournal
                    {
                        EntryNo = order.Id,
                        EntryLineNo = 3, // Adjust if needed
                        Status = item.IsVoid ? "Unposted" : "Posted",
                        EntryName = item.EntryId ?? "",
                        AccountName = accountName,
                        EntryDate = item.createdAt.DateTime,
                        Description = description,
                        QtyOut = item.ItemQTY,
                        Price = Convert.ToDouble(item.ItemPrice)
                    };

                    journals.Add(journal);

                    _logger.LogInformation("Prepared journal entry: AccountName={AccountName}, Description={Description}, QtyOut={Qty}, Price={Price}",
                        journal.AccountName, journal.Description, journal.QtyOut, journal.Price);
                }

                if (!journals.Any())
                {
                    _logger.LogWarning("No valid journal entries generated for Order {OrderId}", orderId);
                    return (false, "No valid journal entries to add.");
                }

                _journal.AccountJournal.AddRange(journals);
                await _journal.SaveChangesAsync();

                _logger.LogInformation("Successfully added {Count} journal entries for Order {OrderId}.", journals.Count, orderId);
                return (true, "Journal entries successfully added.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding journal entries for Order {OrderId}.", orderId);
                return (false, $"An error occurred: {ex.Message}");
            }
        }

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

        public async Task<(bool isSuccess, string message)> AddPwdScJournal(long orderId)
        {
            _logger.LogInformation("Starting AddPwdScJournal for OrderId: {OrderId}", orderId);

            if (orderId <= 0)
            {
                _logger.LogWarning("Invalid OrderId: {OrderId}", orderId);
                return (false, "Invalid order ID.");
            }

            // 1) Load the order so we can read the PWD/SC fields
            var order = await _dataContext.Order
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                _logger.LogWarning("Order with ID {OrderId} not found.", orderId);
                return (false, "Order not found.");
            }

            // 2) Guard: need both names and OSCAs
            if (string.IsNullOrWhiteSpace(order.EligiblePwdScNames) ||
                string.IsNullOrWhiteSpace(order.OSCAIdsNum))
            {
                _logger.LogWarning("No PWD/SC data on Order {OrderId}. Names: '{Names}', OSCAs: '{Oscas}'",
                    orderId, order.EligiblePwdScNames, order.OSCAIdsNum);
                return (false, "No PWD/SC information to journal.");
            }

            // 3) Split into lists
            var names = order.EligiblePwdScNames
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(n => n.Trim())
                .ToList();

            var oscas = order.OSCAIdsNum
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(o => o.Trim())
                .ToList();

            // 4) Pair them up to the smaller count
            var count = Math.Min(names.Count, oscas.Count);
            if (count == 0)
            {
                _logger.LogWarning("After splitting, no valid PWD/SC pairs for Order {OrderId}.", orderId);
                return (false, "No valid PWD/SC pairs found.");
            }

            // 5) Build journal entries
            var journals = new List<AccountJournal>();
            int lineNo = 5;  // starting line number for PWD/SC entries

            for (int i = 0; i < count; i++)
            {
                var name = names[i];
                var osca = oscas[i];

                var journal = new AccountJournal
                {
                    EntryNo = order.Id,
                    EntryLineNo = lineNo++,
                    Status = order.IsCancelled ? "Unposted" : "Posted",
                    AccountName = name,
                    Reference = osca,
                    EntryName = order.DiscountType ?? "",
                    EntryDate = order.CreatedAt.DateTime   // assuming CreatedAt is DateTime
                };

                journals.Add(journal);

                _logger.LogInformation(
                    "Prepared PWD/SC journal #{LineNo}: Name={Name}, OSCA={Osca}",
                    journal.EntryLineNo, name, osca);
            }

            // 6) Persist
            try
            {
                _journal.AccountJournal.AddRange(journals);
                await _journal.SaveChangesAsync();

                _logger.LogInformation(
                    "Successfully added {Count} PWD/SC journal entries for Order {OrderId}.",
                    journals.Count, orderId);

                return (true, $"{journals.Count} PWD/SC entries added.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error saving PWD/SC journal entries for Order {OrderId}.",
                    orderId);

                return (false, $"An error occurred: {ex.Message}");
            }
        }


        public async Task<(bool isSuccess, string message)> AddTendersJournal(long orderId)
        {
            _logger.LogInformation("Starting AddTendersJournal for OrderId: {OrderId}", orderId);

            if (orderId <= 0)
            {
                _logger.LogWarning("Invalid OrderId: {OrderId}", orderId);
                return (false, "Invalid order ID.");
            }

            // Load the order plus any AlternativePayments
            var order = await _dataContext.Order
                .Include(o => o.AlternativePayments)
                    .ThenInclude(t => t.SaleType)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                _logger.LogWarning("Order with ID {OrderId} not found.", orderId);
                return (false, "Order not found");
            }

            var journals = new List<AccountJournal>();

            // 1) Record the cash tendered on the order itself
            if (order.CashTendered > 0)
            {
                journals.Add(new AccountJournal
                {
                    EntryNo = order.Id,
                    EntryLineNo = 0,
                    Status = order.IsCancelled ? "Unposted" : "Posted",
                    EntryName = "Cash Tendered",
                    AccountName = "Cash",                   // or pull from a config/account‑mapping
                    Description = "Cash Tendered",
                    Debit = Convert.ToDouble(order.CashTendered),
                    EntryDate = order.CreatedAt.DateTime           // assuming CreatedAt is DateTime
                });
            }

            // 2) Record any alternative payments (card, gift‑card, etc.)
            if (order.AlternativePayments != null)
            {
                foreach (var tender in order.AlternativePayments)
                {

                    var journal = new AccountJournal
                    {
                        EntryNo = order.Id,
                        EntryLineNo = 0,
                        Status = order.IsCancelled ? "Unposted" : "Posted",
                        EntryName = tender.SaleType.Name,
                        AccountName = tender.SaleType.Account,
                        Description = tender.SaleType.Type,
                        Reference = tender.Reference,
                        Credit = Convert.ToDouble(tender.Amount),
                        EntryDate = order.CreatedAt.DateTime
                    };

                    journals.Add(journal);

                    _logger.LogInformation(
                        "Prepared tender journal #{LineNo}: Account={Account}, Credit={Credit}",
                        journal.EntryLineNo, journal.AccountName, journal.Credit);
                }
            }

            if (!journals.Any())
            {
                _logger.LogWarning("No payment entries to journal for Order {OrderId}.", orderId);
                return (false, "No payment entries found.");
            }

            try
            {
                _journal.AccountJournal.AddRange(journals);
                await _journal.SaveChangesAsync();

                _logger.LogInformation(
                    "Successfully added {Count} payment journal entries for Order {OrderId}.",
                    journals.Count, orderId);

                return (true, $"{journals.Count} payment entries added.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error saving payment journal entries for Order {OrderId}.",
                    orderId);

                return (false, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<(bool isSuccess, string message)> AddTotalsJournal(long orderId)
        {
            _logger.LogInformation("Starting AddTotalsJournal for OrderId: {OrderId}", orderId);

            if (orderId <= 0)
            {
                _logger.LogWarning("Invalid OrderId: {OrderId}", orderId);
                return (false, "Invalid order ID.");
            }

            // Load just the Order so we can grab TotalAmount, DiscountType, DiscountAmount
            var order = await _dataContext.Order
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                _logger.LogWarning("Order with ID {OrderId} not found.", orderId);
                return (false, "Order not found.");
            }

            var journals = new List<AccountJournal>();

            // 1) Discount line (EntryLineNo = 9)
            if (order.DiscountAmount > 0)
            {
                var discountAccount = !string.IsNullOrWhiteSpace(order.DiscountType)
                    ? order.DiscountType
                    : "Discount";

                journals.Add(new AccountJournal
                {
                    EntryNo = order.Id,
                    EntryLineNo = 10,
                    EntryName = "Discount Amount",
                    Status = order.IsCancelled ? "Unposted" : "Posted",
                    AccountName = discountAccount,
                    Description = "Discount",
                    Debit = Convert.ToDouble(order.DiscountAmount),
                    EntryDate = order.CreatedAt.DateTime    // assuming CreatedAt is DateTime
                });

                _logger.LogInformation(
                    "Prepared discount journal (Line 9): Account={Account}, Debit={Amt}",
                    discountAccount, order.DiscountAmount);
            }

            // 2) Total line (EntryLineNo = 10)
            journals.Add(new AccountJournal
            {
                EntryNo = order.Id,
                EntryLineNo = 10,
                EntryName = "Total Amount",
                Status = order.IsCancelled ? "Unposted" : "Posted",
                AccountName = "Sales",           // change to your revenue GL account
                Description = "Order Total",
                Credit = Convert.ToDouble(order.TotalAmount),
                EntryDate = order.CreatedAt.DateTime
            });

            _logger.LogInformation(
                "Prepared total journal (Line 10): Account=Sales, Credit={Amt}",
                order.TotalAmount);

            if (!journals.Any())
            {
                _logger.LogWarning("No totals entries to journal for Order {OrderId}.", orderId);
                return (false, "No totals to journal.");
            }

            try
            {
                _journal.AccountJournal.AddRange(journals);
                await _journal.SaveChangesAsync();

                _logger.LogInformation(
                    "Successfully added {Count} totals journal entries for Order {OrderId}.",
                    journals.Count, orderId);

                return (true, $"{journals.Count} totals entries added.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error saving totals journal entries for Order {OrderId}.",
                    orderId);

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
