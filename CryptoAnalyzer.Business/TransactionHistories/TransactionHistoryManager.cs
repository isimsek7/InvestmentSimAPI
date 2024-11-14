using System.Security.Claims;
using CryptoAnalyzer.Data.Context;
using CryptoAnalyzer.Data.Entities;
using CryptoAnalyzer.Data.Repositories;
using CryptoAnalyzer.Business.Operations.User;
using Microsoft.EntityFrameworkCore;

namespace CryptoAnalyzer.Business.Operations.TransactionHistories
{
    public class TransactionHistoryManager : ITransactionHistoryService
    {
        private readonly IRepository<InvestmentEntity> _investmentRepository;
        private readonly CryptoAnalyzerDbContext _context;

        public TransactionHistoryManager(IRepository<InvestmentEntity> investmentRepository,CryptoAnalyzerDbContext context)
                                          // Added IUserService
        {
            _investmentRepository = investmentRepository;
            _context = context;
        }

        public async Task<IEnumerable<TransactionHistoryEntity?>> GetUserTransactionHistoryAsync(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            // Parse the user ID from the claims
            int userId = int.Parse(userIdClaim.Value);

            // Fetch investments for the user
            var investments = await _investmentRepository
                .GetAll() // Assuming GetAll() retrieves all investments
                .Where(investment => investment.UserId == userId)
                .ToListAsync();

            // If no investments are found, return an empty collection
            if (investments == null || !investments.Any())
            {
                return Enumerable.Empty<TransactionHistoryEntity?>(); // No investments found, return empty
            }

            // Fetch transaction histories linked to those investments
            var transactionHistories = await _context.TransactionHistories
                .Where(th => investments.Select(i => i.Id).Contains(th.InvestmentId) && th.IsDeleted == false).ToListAsync();// Only get non-deleted transactions
                

            return transactionHistories ?? Enumerable.Empty<TransactionHistoryEntity?>(); // Return transaction histories or empty
        }

    }
}
