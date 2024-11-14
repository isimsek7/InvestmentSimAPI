using System;
using CryptoAnalyzer.Business.Types;
using System.Security.Claims;
using CryptoAnalyzer.Business.UserInvestmentGroups.Dtos;
using CryptoAnalyzer.Data.Entities;

namespace CryptoAnalyzer.Business.UserInvestmentGroups
{
	public interface IUserInvestmentGroupsService
	{
        Task<IEnumerable<UserInvestmentGroupDto>> GetInvestmentGroupsWithUsersAsync();
        Task<ServiceMessage> LeaveInvestmentGroupAsync(int groupId, ClaimsPrincipal user);

    }
}

