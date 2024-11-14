using CryptoAnalyzer.Data.Repositories;
using CryptoAnalyzer.Business.Operations.Portfolios.Dtos;
using CryptoAnalyzer.Business.Types;
using CryptoAnalyzer.Business.Operations.Portfolios;
using CryptoAnalyzer.Data.Entities;
using CryptoAnalyzer.Data.UnitOfWork;
using System.Security.Claims;
using CryptoAnalyzer.Data.Context;

public class PortfolioService : IPortfolioService
{
    private readonly IRepository<PortfolioEntity> _portfolioRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PortfolioService(IRepository<PortfolioEntity> portfolioRepository, IUnitOfWork unitOfWork, IRepository<UserEntity> userRepository, CryptoAnalyzerDbContext _context)
    {
        _portfolioRepository = portfolioRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceMessage> CreatePortfolioAsync(CreatePortfolioDto createPortfolioDto, ClaimsPrincipal user)
    {
        // Get the logged-in user's ID from claims
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return new ServiceMessage { IsSucceeded = false, Message = "User not found." };
        }

        int userId = int.Parse(userIdClaim.Value);

       
        // Check if a portfolio with the same name exists for this user
        var portfolioExists = await _portfolioRepository.ExistsAsync(p => p.UserId == userId && p.Name.Trim().ToLower() == createPortfolioDto.Name.Trim().ToLower());

        if (portfolioExists)
        {
            return new ServiceMessage { IsSucceeded = false, Message = "Portfolio with this name already exists." };
        }

        // Create a new portfolio entity
        var portfolio = new PortfolioEntity
        {

            UserId=userId,
            Name = createPortfolioDto.Name,
            Description = createPortfolioDto.Description,
        };

        // Add portfolio to the repository
        _portfolioRepository.Add(portfolio);

        // Save changes
        await _unitOfWork.SaveChangesAsync();

        return new ServiceMessage { IsSucceeded = true, Message = "Portfolio created successfully." };
    }

    public async Task<IEnumerable<PortfolioEntity?>> GetUserPortfolioAsync(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("Nothing found.");
        }

        int userId = int.Parse(userIdClaim.Value);

        var portfolio = _portfolioRepository.GetAll().Where(x => x.UserId == userId);
        return portfolio;
    }

}



