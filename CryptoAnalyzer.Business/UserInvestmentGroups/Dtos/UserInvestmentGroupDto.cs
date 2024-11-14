using System;
namespace CryptoAnalyzer.Business.UserInvestmentGroups.Dtos
{
    public class UserInvestmentGroupDto
    {
        public int InvestmentGroupId { get; set; }
        public string GroupName { get; set; }
        public List<string> UserNames { get; set; } = new List<string>();
    }
}
