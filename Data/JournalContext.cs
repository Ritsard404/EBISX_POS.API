using EBISX_POS.API.Models;
using EBISX_POS.API.Models.Journal;
using Microsoft.EntityFrameworkCore;

namespace EBISX_POS.API.Data
{
    public class JournalContext : DbContext
    {
        public JournalContext(DbContextOptions<JournalContext> options) : base(options)
        {

        }
        public DbSet<AccountJournal> AccountJournal { get; set; }
        public DbSet<SalesJournal> SalesJournal { get; set; }
    }
}
