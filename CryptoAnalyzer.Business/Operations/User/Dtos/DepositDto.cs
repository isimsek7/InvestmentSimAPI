using System;
namespace CryptoAnalyzer.Business.Operations.User.Dtos
{
    public class DepositDto
    {
        public int UserId { get; set; }
        public decimal DepositAmount { get; set; }
    }
}

