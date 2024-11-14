using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoAnalyzer.Data.Entities
{
	public class UserInvestmentGroupEntity : BaseEntity
    {
        public int UserId { get; set; }
        public int InvestmentGroupId { get; set; }

        // Navigation properties
        public virtual UserEntity User { get; set; }
        public virtual InvestmentGroupEntity InvestmentGroup { get; set; }
    }
    public class UserInvestmentGroupConfiguration : BaseConfiguration<UserInvestmentGroupEntity>
    {
        public override void Configure(EntityTypeBuilder<UserInvestmentGroupEntity> builder)
        {
            builder.HasKey(x => new { x.UserId, x.InvestmentGroupId });
            base.Configure(builder);
        }
    }
}

