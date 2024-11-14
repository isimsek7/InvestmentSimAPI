using System.Security.Claims;
using CryptoAnalyzer.Business.Operations.Investments.Dtos;
using CryptoAnalyzer.Business.Types;
using CryptoAnalyzer.Data.Entities;
using CryptoAnalyzer.Data.Repositories;
using CryptoAnalyzer.Data.UnitOfWork;

namespace CryptoAnalyzer.Business.Operations.Investments
{
    public class InvestmentService : IInvestmentService
    {
        private readonly IRepository<InvestmentEntity> _investmentRepository;
        private readonly IRepository<TransactionHistoryEntity> _transactionHistoryRepository;
        private readonly IRepository<PortfolioEntity> _portfolioRepository;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IUnitOfWork _unitOfWork;


        public InvestmentService(
            IRepository<InvestmentEntity> investmentRepository,
            IRepository<TransactionHistoryEntity> transactionHistoryRepository,
            IRepository<PortfolioEntity> portfolioRepository,
            IRepository<UserEntity> userRepository,
            IUnitOfWork unitOfWork)
        {
            _investmentRepository = investmentRepository;
            _transactionHistoryRepository = transactionHistoryRepository;
            _portfolioRepository = portfolioRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceMessage> BuyInvestmentAsync(BuyInvestmentDto buyInvestmentDto, ClaimsPrincipal user)
        {
            // Extract user ID from claims
            var userIdString = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null)
            {
                return new ServiceMessage { IsSucceeded = false, Message = "User not authenticated." };
            }

            int userId = Convert.ToInt32(userIdString);

            // Validate the portfolio exists
            var portfolio = await _portfolioRepository.GetByIdAsync(buyInvestmentDto.PortfolioId);
            if (portfolio == null || portfolio.UserId != userId) // Ensure the portfolio belongs to the logged-in user
            {
                return new ServiceMessage { IsSucceeded = false, Message = "Portfolio not found." };
            }

            // Check user's balance
            var userEntity = await _userRepository.GetByIdAsync(userId);
            if (userEntity == null)
            {
                return new ServiceMessage { IsSucceeded = false, Message = "User not found." };
            }

            decimal totalCost = buyInvestmentDto.PriceAtPurchase * buyInvestmentDto.Amount;
            if (userEntity.Deposit < totalCost)
            {
                return new ServiceMessage { IsSucceeded = false, Message = "Insufficient funds." };
            }

            // Create the investment entity
            try
            {
                await _unitOfWork.BeginTransaction();

                var investment = new InvestmentEntity
                {
                    UserId = userId,
                    PortfolioId = buyInvestmentDto.PortfolioId,
                    Asset = buyInvestmentDto.Asset,
                    Amount = buyInvestmentDto.Amount,
                    PriceAtPurchase = buyInvestmentDto.PriceAtPurchase,

                };

                _investmentRepository.Add(investment);

                await _unitOfWork.SaveChangesAsync();

                // Create transaction history
                var transaction = new TransactionHistoryEntity
                {
                    InvestmentId = investment.Id, // Assume this ID will be set after saving
                    PortfolioId = investment.PortfolioId,
                    Action = "Buy",
                    Asset = buyInvestmentDto.Asset,
                    Amount = buyInvestmentDto.Amount,
                    Price = buyInvestmentDto.Price,
                    PriceAtPurchase = buyInvestmentDto.PriceAtPurchase,
                    CreatedDate = DateTime.UtcNow
                };

                _transactionHistoryRepository.Add(transaction);

                // Deduct from user balance
                userEntity.Deposit -= totalCost;
                _userRepository.Update(userEntity);

                // Save changes
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch(Exception e)
            {
                e.Message.ToString();
                _unitOfWork.RollBackTransaction();
            }

            return new ServiceMessage { IsSucceeded = true, Message = "Investment purchased successfully." };
        }

        public async Task<ServiceMessage> SellInvestmentAsync(SellInvestmentDto sellInvestmentDto, ClaimsPrincipal user)
        {
            // Extract user ID from claims
            var userIdString = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null)
            {
                return new ServiceMessage { IsSucceeded = false, Message = "User not authenticated." };
            }

            int userId = Convert.ToInt32(userIdString);

            // Validate the investment exists and belongs to the user
            var investment = await _investmentRepository.GetByIdAsync(sellInvestmentDto.InvestmentId);
            if (investment == null)
            {
                return new ServiceMessage { IsSucceeded = false, Message = "Investment not found." };
            }

            // Ensure the investment belongs to the user
            if (investment.UserId != userId)
            {
                return new ServiceMessage { IsSucceeded = false, Message = "Investment does not belong to the user." };
            }

            // Validate the sell amount
            if (sellInvestmentDto.Amount > investment.Amount)
            {
                return new ServiceMessage { IsSucceeded = false, Message = "Insufficient investment amount to sell." };
            }

            // Update investment amount
            investment.Amount -= sellInvestmentDto.Amount;
            if (investment.Amount == 0)
            {
                // Delete investment if sold out
                _investmentRepository.Delete(investment);
            }
            else
            {
                _investmentRepository.Update(investment);
            }

            // Create transaction history using PriceAtPurchase from the investment entity
            var transaction = new TransactionHistoryEntity
            {
                InvestmentId = investment.Id,
                Asset = investment.Asset,
                PortfolioId=investment.PortfolioId,// Use asset from the existing investment
                Action = "Sell",
                Amount = sellInvestmentDto.Amount,
                Price = sellInvestmentDto.SellingPrice,
                PriceAtPurchase = investment.PriceAtPurchase, // Use PriceAtPurchase from the investment
                Date = DateTime.UtcNow
            };
            _transactionHistoryRepository.Add(transaction);

            // Update user balance (add the selling amount)
            var userEntity = await _userRepository.GetByIdAsync(userId);
            userEntity.Deposit += sellInvestmentDto.SellingPrice * sellInvestmentDto.Amount;
            _userRepository.Update(userEntity);

            // Save changes
            await _unitOfWork.SaveChangesAsync();

            return new ServiceMessage { IsSucceeded = true, Message = "Investment sold successfully." };
        }

        public async Task<ServiceMessage> UpdateInvestmentAsync(UpdateInvestmentDto updateInvestmentDto)
        {
            // Start a new transaction
            await _unitOfWork.BeginTransaction();
            try
            {
                //Retrieve the existing investment
                var existingInvestment = await _investmentRepository.GetByIdAsync(updateInvestmentDto.InvestmentId);
                if (existingInvestment == null)
                {
                    return new ServiceMessage { IsSucceeded = false, Message = "Investment not found." };
                }

                //Delete the existing transaction history
                await _transactionHistoryRepository.DeleteByInvestmentIdAsync(existingInvestment.Id);

                //Soft delete the existing investment
                existingInvestment.IsDeleted = true;
                _investmentRepository.Update(existingInvestment);

                //new investment entity
                var newInvestment = new InvestmentEntity
                {
                    UserId = existingInvestment.UserId,
                    PortfolioId = updateInvestmentDto.PortfolioId,
                    Asset = existingInvestment.Asset,
                    Amount = updateInvestmentDto.Amount,
                    PriceAtPurchase = updateInvestmentDto.PriceAtPurchase,
                    Price = updateInvestmentDto.Price,
                    ModifiedDate = DateTime.UtcNow,
                    CreatedDate=existingInvestment.CreatedDate,
                };

                _investmentRepository.Add(newInvestment);
                await _unitOfWork.SaveChangesAsync(); // Save to generate the new Investment ID

                // Create a new transaction history entry
                var newTransaction = new TransactionHistoryEntity
                {
                    InvestmentId = newInvestment.Id,
                    PortfolioId = updateInvestmentDto.PortfolioId,
                    Asset = updateInvestmentDto.Asset,
                    Action = updateInvestmentDto.Action,
                    Amount = updateInvestmentDto.Amount,
                    PriceAtPurchase = updateInvestmentDto.PriceAtPurchase,
                    Price = updateInvestmentDto.Price,
                    Date = DateTime.UtcNow,
                };

                _transactionHistoryRepository.Add(newTransaction);

                // Save changes for the new transaction history
                await _unitOfWork.SaveChangesAsync();

                // Commit the transaction
                await _unitOfWork.CommitTransaction();

                return new ServiceMessage { IsSucceeded = true, Message = "Investment updated successfully." };
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                await _unitOfWork.RollBackTransaction();
                return new ServiceMessage { IsSucceeded = false, Message = $"An error occurred: {ex.Message}" };
            }
        }
    }
}



