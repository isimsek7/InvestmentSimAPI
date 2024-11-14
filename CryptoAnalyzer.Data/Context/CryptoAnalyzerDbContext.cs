
using CryptoAnalyzer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoAnalyzer.Data.Context
{
    public class CryptoAnalyzerDbContext : DbContext
    {
        public CryptoAnalyzerDbContext(DbContextOptions<CryptoAnalyzerDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations for each entity
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PortfolioConfiguration());
            modelBuilder.ApplyConfiguration(new InvestmentConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new InvestmentGroupConfiguration());
            modelBuilder.ApplyConfiguration(new UserInvestmentGroupConfiguration());

            modelBuilder.Entity<SettingEntity>().HasData(
            new SettingEntity
            {
                Id = 1,
                MaintenanceMode = false
            });
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<PortfolioEntity> Portfolios => Set<PortfolioEntity>();
        public DbSet<InvestmentEntity> Investments => Set<InvestmentEntity>();
        public DbSet<TransactionHistoryEntity> TransactionHistories => Set<TransactionHistoryEntity>();
        public DbSet<InvestmentGroupEntity> InvestmentGroups => Set<InvestmentGroupEntity>();
        public DbSet<UserInvestmentGroupEntity> UserInvestmentGroups => Set<UserInvestmentGroupEntity>();
        public DbSet<SettingEntity> Settings => Set<SettingEntity>();
    }
}
    


