using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoAnalyzer.Data.Entities
{
    public class TransactionHistoryEntity : BaseEntity
    {
        public int InvestmentId { get; set; }
        public int PortfolioId { get; set; }
        public string Action { get; set; }
        public decimal Amount { get; set; }
        public string Asset { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public decimal Price { get; set; }
        [JsonIgnore]
        public DateTime Date { get; set; }//postpone
    }

    public class TransactionHistoryConfiguration : BaseConfiguration<TransactionHistoryEntity>
    {
        public override void Configure(EntityTypeBuilder<TransactionHistoryEntity> builder)
        {
            
            base.Configure(builder);
        }
    }
}
