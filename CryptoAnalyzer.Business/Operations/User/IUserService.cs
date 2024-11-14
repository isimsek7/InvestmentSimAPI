using System;
using System.Security.Claims;
using CryptoAnalyzer.Business.Operations.User.Dtos;
using CryptoAnalyzer.Business.Types;

namespace CryptoAnalyzer.Business.Operations.User
{
	public interface IUserService
	{
		Task<ServiceMessage> AddUser(AddUserDto user);

		ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user);

		Task<DepositDto> GetUserDepositAsync(ClaimsPrincipal user);

    }
}

