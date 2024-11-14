using System;
namespace CryptoAnalyzer.Business.Operations.TransactionHistories.Dtos
{
    public class EditTransactionDto
    {
        public int Id { get; set; } // Transaction ID
        public int UserId { get; set; }
        public int InvestmentId { get; set; } // Investment ID
        public string Asset { get; set; }
        public decimal Amount { get; set; } // Updated amount
        public decimal PriceAtPurchase { get; set; } // Updated price at purchase
        public DateTime Date { get; set; } // Updated date
    }
}

