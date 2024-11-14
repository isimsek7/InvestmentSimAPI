using CryptoAnalyzer.Business.Operations.User;
using CryptoAnalyzer.Business.Operations.User.Dtos;
using CryptoAnalyzer.WebApi.JwtHelper;
using CryptoAnalyzer.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CryptoAnalyzer.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly ILogger<PortfolioService> _logger;

        public AuthController(IUserService userService, ILogger<PortfolioService> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addUserDto = new AddUserDto
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                BirthDate = request.BirthDate,
            };

            var result = await _userService.AddUser(addUserDto);

            if (result.IsSucceeded)
                return Ok();
            else
                return BadRequest(result.Message);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userService.LoginUser(new LoginUserDto { Email = request.Email, Password = request.Password });

            if (!result.IsSucceeded)
                return BadRequest(result.Message);

            var user = result.Data;

            // Logging the user information for debugging
            _logger.LogInformation($"Login Successful: User ID: {user.Id}, Email: {user.Email}");

            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            // Ensure user ID is not zero or invalid
            if (user.Id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var token = JwtHelper.JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,  // Ensure this value is correct
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType,
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                ExpireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)
            });

            return Ok(new LoginResponse { Messsage = "Login Successful", Token = token });
        }

        [HttpGet("my-funds")]
        [Authorize(Roles="User")]
        public async Task<ActionResult<DepositDto>> GetUserDeposit()
        {
            try
            {
                var depositInfo = await _userService.GetUserDepositAsync(User);
                return Ok(depositInfo);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
