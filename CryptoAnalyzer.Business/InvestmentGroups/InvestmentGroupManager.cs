using System.Security.Claims;
using CryptoAnalyzer.Business.InvestmentGroups;
using CryptoAnalyzer.Business.Types;
using CryptoAnalyzer.Data.Entities;
using CryptoAnalyzer.Data.Repositories;
using CryptoAnalyzer.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

public class InvestmentGroupManager : IInvestmentGroupService
{
    private readonly IRepository<InvestmentGroupEntity> _investmentGroups;
    private readonly IRepository<UserInvestmentGroupEntity> _userInvestmentGroups;
    private readonly IUnitOfWork _unitOfWork;

    public InvestmentGroupManager(IRepository<InvestmentGroupEntity> investmentGroups, IUnitOfWork unitOfWork,
        IRepository<UserInvestmentGroupEntity> userInvestmentGroups)
    {
        _investmentGroups = investmentGroups;
        _unitOfWork = unitOfWork;
        _userInvestmentGroups = userInvestmentGroups;
    }

    public async Task<ServiceMessage> CreateInvestmentGroup(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
        {
            return new ServiceMessage { IsSucceeded = false, Message = "Investment group name cannot be empty!" };
        }

        var newGroup = new InvestmentGroupEntity
        {
            Name = groupName,
            CreatedDate = DateTime.Now
        };

        _investmentGroups.Add(newGroup);
        await _unitOfWork.SaveChangesAsync();  // Call SaveChanges synchronously
        return new ServiceMessage { IsSucceeded = true, Message = "Investment group created successfully!" };
    }

    public IEnumerable<InvestmentGroupEntity> GetAllInvestmentGroups()
    {
        return _investmentGroups.GetAll();
    }

    public async Task<ServiceMessage> EditInvestmentGroupAsync(int id, string newGroupName)
    {
        if (string.IsNullOrWhiteSpace(newGroupName))
        {
            return new ServiceMessage { IsSucceeded = false, Message = "Investment group name cannot be empty!" };
        }

        var existingGroup = await _investmentGroups.GetByIdAsync(id);
        if (existingGroup == null)
        {
            return new ServiceMessage { IsSucceeded = false, Message = "Investment group not found!" };
        }

        existingGroup.Name = newGroupName;
        _investmentGroups.Update(existingGroup);

        await _unitOfWork.SaveChangesAsync();

        return new ServiceMessage { IsSucceeded = true, Message = "Investment group updated successfully!" };
    }

    public ServiceMessage DeleteInvestmentGroup(int id, bool softDelete = true)
    {
        var existingGroup = _investmentGroups.GetById(id);

        if (existingGroup == null)
        {
            return new ServiceMessage { IsSucceeded = false, Message = "Investment group not found!" };
        }

        // Perform deletion based on the softDelete flag
        _investmentGroups.Delete(existingGroup, softDelete);

        // Save changes using UnitOfWork
        _unitOfWork.SaveChangesAsync(); // Assuming this is synchronous

        return new ServiceMessage { IsSucceeded = true, Message = "Investment group deleted successfully!" };
    }

    public async Task<ServiceMessage> JoinInvestmentGroupAsync(string groupName, ClaimsPrincipal user)
    {
        var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var investmentGroup = await _investmentGroups.GetAsync(g => g.Name == groupName);
        if (investmentGroup == null)
        {
            return new ServiceMessage { IsSucceeded = false, Message = "Investment group not found!" };
        }

        var userInvestmentGroup = new UserInvestmentGroupEntity
        {
            UserId = userId,
            InvestmentGroupId = investmentGroup.Id
        };

        _userInvestmentGroups.Add(userInvestmentGroup);
        await _unitOfWork.SaveChangesAsync();

        return new ServiceMessage { IsSucceeded = true, Message = "Successfully joined the investment group!" };
    }
}
