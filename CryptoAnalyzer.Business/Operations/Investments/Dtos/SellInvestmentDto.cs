using System;
namespace CryptoAnalyzer.Business.Operations.Investments.Dtos
{
    public class SellInvestmentDto
    {
        public int InvestmentId { get; set; }
        public decimal Amount { get; set; }
        public decimal SellingPrice { get; set; }
    }
}

