using CryptoAnalyzer.Business.Operations.Investments;
using CryptoAnalyzer.Business.Operations.Investments.Dtos;
using CryptoAnalyzer.Business.Operations.TransactionHistories;
using CryptoAnalyzer.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class InvestmentController : ControllerBase
{
    private readonly IInvestmentService _investmentService;

    public InvestmentController(IInvestmentService investmentService, ITransactionHistoryService transactionHistoryService)
    {
        _investmentService = investmentService;
    }
    [HttpPost("buy")]
    [Authorize(Roles ="User")]
    //[TimeControlFilter]
    
    public async Task<IActionResult> BuyInvestment([FromBody] BuyInvestmentDto buyInvestmentDto)
    {
        // Fetch the user ID from the claims (based on the logged-in user)
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("User not found.");
        }

        // Call the service without exposing userId in the API
        var result = await _investmentService.BuyInvestmentAsync(buyInvestmentDto, User);

        if (!result.IsSucceeded)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    [HttpPost("sell")]
    [Authorize(Roles ="User")]
    //[TimeControlFilter]
    public async Task<IActionResult> SellInvestmentAsync([FromBody] SellInvestmentDto sellInvestmentDto)
    {
        if (sellInvestmentDto == null || !ModelState.IsValid)
        {
            return BadRequest("Invalid sell data.");
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("User not found.");
        }

        var result = await _investmentService.SellInvestmentAsync(sellInvestmentDto, User);

        if (!result.IsSucceeded)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    [HttpPatch("update-transaction")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateInvestmentAsync([FromBody]UpdateInvestmentDto updateInvestmentDto)
    {
        var result = await _investmentService.UpdateInvestmentAsync(updateInvestmentDto);
        if (!result.IsSucceeded)
        {
            return NotFound(result.Message);
        }

        return Ok(result.Message);
    }
}