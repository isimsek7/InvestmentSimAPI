using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoAnalyzer.Data.Entities
{
	public class PortfolioEntity:BaseEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public virtual UserEntity User { get; set; }
    }
    public class PortfolioConfiguration : BaseConfiguration<PortfolioEntity>
    {
        public override void Configure(EntityTypeBuilder<PortfolioEntity> builder)
        {
            base.Configure(builder);
        }
    }
}

