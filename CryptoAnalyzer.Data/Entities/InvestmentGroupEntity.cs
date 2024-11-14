using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoAnalyzer.Data.Entities
{
	public class InvestmentGroupEntity:BaseEntity
	{
        public string Name { get; set; }

        public virtual ICollection<UserInvestmentGroupEntity> UserInvestmentGroups { get; set; }
    }

    public class InvestmentGroupConfiguration : BaseConfiguration<InvestmentGroupEntity>
    {
        public override void Configure(EntityTypeBuilder<InvestmentGroupEntity> builder)
        {
            base.Configure(builder);
        }
    }
}

