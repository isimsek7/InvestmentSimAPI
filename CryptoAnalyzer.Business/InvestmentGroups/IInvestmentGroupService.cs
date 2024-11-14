using System;
using System.Security.Claims;
using CryptoAnalyzer.Business.Types;
using CryptoAnalyzer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoAnalyzer.Business.InvestmentGroups
{
	public interface IInvestmentGroupService
	{
        public Task<ServiceMessage> CreateInvestmentGroup(string groupName);
        public IEnumerable<InvestmentGroupEntity> GetAllInvestmentGroups();
        public Task<ServiceMessage> EditInvestmentGroupAsync(int id, string newGroupName);
        public ServiceMessage DeleteInvestmentGroup(int id, bool softDelete = true);
        Task<ServiceMessage> JoinInvestmentGroupAsync(string groupName, ClaimsPrincipal user);

    }


}

