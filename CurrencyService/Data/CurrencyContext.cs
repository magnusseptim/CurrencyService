using CurrencyService.Model;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Contexts
{
    public class CurrencyContext : DbContext
    {
        public CurrencyContext(DbContextOptions<CurrencyContext> options) : base(options)
        {
        }


        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyDate> CurrencyDates { get; set; }
        public DbSet<Path> Paths { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Currency>()
                   .HasOne(x => x.CurrencyDate)
                   .WithMany(y => y.Currencies)
                   .HasForeignKey(z=>z.CurrencyDateID);

            builder.Entity<Path>()
                   .ToTable("Paths");

            builder.Entity<Currency>()
                   .ToTable("Currencies");

            builder.Entity<CurrencyDate>()
                   .ToTable("CurrencyDates");

            builder.Entity<Log>()
                   .HasKey(x => x.Id);

            builder.Entity<Log>()
                   .ToTable("Logs"); 
        }
    }
}
