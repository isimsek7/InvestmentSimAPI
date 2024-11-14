using CryptoAnalyzer.Business.Operations.TransactionHistories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CryptoAnalyzer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionHistoryController : ControllerBase
    {
        private readonly ITransactionHistoryService _transactionHistoryService;

        public TransactionHistoryController(ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetUserTransactionHistory()
        {
            try
            {
                var transactionHistories = await _transactionHistoryService.GetUserTransactionHistoryAsync(User);

                if (transactionHistories == null || !transactionHistories.Any())
                {
                    return NotFound("No transaction history found.");
                }

                return Ok(transactionHistories);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("User not found or unauthorized.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
