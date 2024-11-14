using System.Security.Claims;
using System.Threading.Tasks;
using CryptoAnalyzer.Business.Operations.Portfolios.Dtos;
using CryptoAnalyzer.Business.Types;
using CryptoAnalyzer.Data.Entities;

namespace CryptoAnalyzer.Business.Operations.Portfolios
{
    public interface IPortfolioService
    {
        Task<ServiceMessage> CreatePortfolioAsync(CreatePortfolioDto createPortfolioDto, ClaimsPrincipal user);
        Task<IEnumerable<PortfolioEntity?>> GetUserPortfolioAsync(ClaimsPrincipal user);
    }
}
