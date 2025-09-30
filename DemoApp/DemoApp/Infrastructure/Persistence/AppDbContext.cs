namespace DemoApp.Infrastructure.Persistence
{
    using DemoApp.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Application EF Core database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">DbContext options.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories => this.Set<Category>();

        public DbSet<Transaction> Transactions => this.Set<Transaction>();

        public DbSet<ExchangeRate> ExchangeRates => this.Set<ExchangeRate>();

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.HasIndex(x => x.Name).IsUnique();
            });

            modelBuilder.Entity<Transaction>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Currency).IsRequired().HasMaxLength(10);
                e.Property(x => x.Amount).HasPrecision(18, 2);
                e.Property(x => x.AmountInHuf).HasPrecision(18, 2);
                e.Property(x => x.RateToHuf).HasPrecision(18, 6);
                e.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
            });

            modelBuilder.Entity<ExchangeRate>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.BaseCurrency).IsRequired().HasMaxLength(10);
                e.Property(x => x.QuoteCurrency).IsRequired().HasMaxLength(10);
                e.Property(x => x.Rate).HasPrecision(18, 6);
                e.HasIndex(x => new { x.Date, x.BaseCurrency, x.QuoteCurrency }).IsUnique();
            });
        }
    }
}
