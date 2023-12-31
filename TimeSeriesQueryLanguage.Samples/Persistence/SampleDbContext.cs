using Microsoft.EntityFrameworkCore;

namespace TimeSeriesQueryLanguage.Samples.Persistence
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
        {
        }

        public DbSet<Ticker> Tickers { get; set; } = default!;
    }
}
