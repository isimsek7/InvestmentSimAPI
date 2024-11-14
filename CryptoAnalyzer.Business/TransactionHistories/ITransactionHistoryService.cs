using System;
using CryptoAnalyzer.Data.Entities;
using System.Security.Claims;
using CryptoAnalyzer.Business.Operations.TransactionHistories.Dtos;
using CryptoAnalyzer.Business.Types;
using System.Threading.Tasks;

namespace CryptoAnalyzer.Business.Operations.TransactionHistories
{
	public interface ITransactionHistoryService
	{
        Task<IEnumerable<TransactionHistoryEntity?>> GetUserTransactionHistoryAsync(ClaimsPrincipal user);
    }
}

