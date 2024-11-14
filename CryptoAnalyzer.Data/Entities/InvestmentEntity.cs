using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoAnalyzer.Data.Entities
{
    public class InvestmentEntity : BaseEntity
    {
        public int UserId { get; set; }
        public int PortfolioId { get; set; }
        public string Asset { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal PriceAtPurchase { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual PortfolioEntity Portfolio { get; set; }
    }
    public class InvestmentConfiguration : BaseConfiguration<InvestmentEntity>
    {
        public override void Configure(EntityTypeBuilder<InvestmentEntity> builder)
        {
            base.Configure(builder);

            builder.HasOne(i => i.User)
                .WithMany()
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Portfolio)
                .WithMany()
                .HasForeignKey(i => i.PortfolioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}


