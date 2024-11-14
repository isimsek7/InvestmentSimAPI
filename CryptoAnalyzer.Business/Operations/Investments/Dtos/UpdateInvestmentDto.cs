using System;
namespace CryptoAnalyzer.Business.Operations.Investments.Dtos
{
    public class UpdateInvestmentDto
    {
        public int UserId { get; set; }
        public int InvestmentId { get; set; }
        public int PortfolioId { get; set; }
        public string Asset { get; set; }
        public decimal Amount { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public string Action { get; set; }
        public decimal Price { get; set; }

    }
}

