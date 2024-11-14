using System;
using CryptoAnalyzer.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoAnalyzer.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public UserType UserType { get; set; }
        public decimal Deposit { get; set; } = 10000;

        public virtual ICollection<UserInvestmentGroupEntity> UserInvestmentGroups { get; set; }

        public virtual ICollection<PortfolioEntity> Portfolios { get; set; }

        public virtual ICollection<InvestmentEntity> Investments { get; set; }
    }

    public class UserConfiguration:BaseConfiguration<UserEntity>
	{
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
			builder.Property(x => x.FirstName).IsRequired().HasMaxLength(27);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(27);
            base.Configure(builder);
        }
    }
}

