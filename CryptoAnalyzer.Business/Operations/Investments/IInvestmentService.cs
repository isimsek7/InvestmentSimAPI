using System.Security.Claims;
using System.Threading.Tasks;
using CryptoAnalyzer.Business.Operations.Investments.Dtos;
using CryptoAnalyzer.Business.Types;
using CryptoAnalyzer.Data.Entities;

namespace CryptoAnalyzer.Business.Operations.Investments
{
    public interface IInvestmentService
    {
        Task<ServiceMessage> BuyInvestmentAsync(BuyInvestmentDto buyInvestmentDto, ClaimsPrincipal user);
        Task<ServiceMessage> SellInvestmentAsync(SellInvestmentDto sellInvestmentDto, ClaimsPrincipal user);
        Task<ServiceMessage> UpdateInvestmentAsync(UpdateInvestmentDto updateInvestmentdto);
      


    }
}
