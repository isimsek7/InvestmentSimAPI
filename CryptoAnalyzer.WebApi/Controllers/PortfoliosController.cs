using CryptoAnalyzer.Business.Operations.Portfolios;
using CryptoAnalyzer.Business.Operations.Portfolios.Dtos;
using CryptoAnalyzer.Data.Entities;
using CryptoAnalyzer.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PortfoliosController : ControllerBase
{
    private readonly IPortfolioService _portfolioService;

    public PortfoliosController(IPortfolioService portfolioService, IRepository<UserEntity> userRepository)
    {
        _portfolioService = portfolioService;

    }

    [HttpPost]
    [Authorize(Roles ="User")]
    public async Task<IActionResult> CreatePortfolio([FromBody] CreatePortfolioDto createPortfolioDto)
    {
        var result = await _portfolioService.CreatePortfolioAsync(createPortfolioDto, User);

        if (!result.IsSucceeded)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }
 
    [HttpGet("user")]
    [Authorize(Roles ="User")]
    public async Task<IActionResult> GetUserPortfolio()
    {
        try
        {
            var portfolio = await _portfolioService.GetUserPortfolioAsync(User);

            if (portfolio == null)
            {
                return NotFound("Portfolio not found.");
            }

            return Ok(portfolio);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

}
