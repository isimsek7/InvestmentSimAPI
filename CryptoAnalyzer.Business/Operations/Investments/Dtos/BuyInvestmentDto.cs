using System;
using System.ComponentModel.DataAnnotations;

namespace CryptoAnalyzer.Business.Operations.Investments.Dtos
{
    public class BuyInvestmentDto
    {
        //public int UserId { get; set; }
        public int PortfolioId { get; set; }
        public string Asset { get; set; }
        public decimal Amount { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public decimal Price { get; set; }
    }
}

