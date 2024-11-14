using System;
using CryptoAnalyzer.Business.Types;
using System.Security.Claims;
using CryptoAnalyzer.Business.UserInvestmentGroups.Dtos;
using CryptoAnalyzer.Data.Context;
using CryptoAnalyzer.Data.Entities;
using CryptoAnalyzer.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using CryptoAnalyzer.Data.UnitOfWork;

namespace CryptoAnalyzer.Business.UserInvestmentGroups
{
    public class UserInvestmentGroupsManager : IUserInvestmentGroupsService
    {

        private readonly IRepository<InvestmentGroupEntity> _investmentGroups;

        private readonly IRepository<UserInvestmentGroupEntity> _userInvestmentGroups;

        private readonly IUnitOfWork _unitOfWork;

        private readonly CryptoAnalyzerDbContext _context;

        public UserInvestmentGroupsManager(IRepository<InvestmentGroupEntity> investmentGroups,
            IRepository<UserInvestmentGroupEntity> userInvestmentGroups, CryptoAnalyzerDbContext context,
            IUnitOfWork unitOfWork)
        {
            _investmentGroups = investmentGroups;
            _userInvestmentGroups = userInvestmentGroups;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserInvestmentGroupDto>> GetInvestmentGroupsWithUsersAsync()
        {
            var investmentGroups = await _investmentGroups.GetAllAsync();

            var result = investmentGroups.Select(group => new UserInvestmentGroupDto
            {
                InvestmentGroupId = group.Id,
                GroupName = group.Name,
                UserNames = _context.UserInvestmentGroups
                    .Where(uig => uig.InvestmentGroupId == group.Id)
                    .Select(uig => $"{uig.User.FirstName} {uig.User.LastName}")
                    .ToList()
            });

            return result;
        }

        public async Task<ServiceMessage> LeaveInvestmentGroupAsync(int groupId, ClaimsPrincipal user)
        {
            // Retrieve the user ID from the claims
            var userIdString = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null)
            {
                return new ServiceMessage { IsSucceeded = false, Message = "User not authenticated." };
            }

            // Convert user ID to an integer
            int userId;
            if (!int.TryParse(userIdString, out userId))
            {
                return new ServiceMessage { IsSucceeded = false, Message = "Invalid user ID." };
            }

            // Check if the user is part of the investment group
            var userInvestmentGroup = await _userInvestmentGroups.GetByUserIdAndGroupIdAsync(userId, groupId);
            if (userInvestmentGroup == null)
            {
                return new ServiceMessage { IsSucceeded = false, Message = "User is not a member of this investment group." };
            }

            // Remove the user from the investment group
             _userInvestmentGroups.Delete(userInvestmentGroup,false);
             await _unitOfWork.SaveChangesAsync();
            return new ServiceMessage { IsSucceeded = true, Message = "Successfully left the investment group." };
        }

    }
    
}

