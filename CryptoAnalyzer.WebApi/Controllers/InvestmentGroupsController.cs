using System;
using CryptoAnalyzer.Business.InvestmentGroups;
using CryptoAnalyzer.Business.InvestmentGroups.Dtos;
using CryptoAnalyzer.Business.Types;
using CryptoAnalyzer.Business.UserInvestmentGroups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoAnalyzer.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class InvestmentGroupsController:ControllerBase
	{
        private readonly IInvestmentGroupService _investmentGroupService;
        private readonly IUserInvestmentGroupsService _userInvestmentGroupService;

        public InvestmentGroupsController(IInvestmentGroupService investmentGroupService,
            IUserInvestmentGroupsService userInvestmentGroupService)
            
        {
            _investmentGroupService = investmentGroupService;
            _userInvestmentGroupService = userInvestmentGroupService;
        }

        [HttpPost("create-group")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateInvestmentGroup([FromBody] CreateGroupDto groupDto)
        {
            var result =  await _investmentGroupService.CreateInvestmentGroup(groupDto.Name);
            if (!result.IsSucceeded)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllInvestmentGroups()
        {
            var investmentGroups =  _investmentGroupService.GetAllInvestmentGroups();
            if (investmentGroups == null || !investmentGroups.Any())
            {
                return NotFound("No investment groups found."); // Return 404 if no groups exist
            }

            // Select the necessary properties to return
            var result = investmentGroups.Select(ig => new
            {
                ig.Id,        // Assuming Id is inherited from BaseEntity
                ig.Name
            });

            return Ok(result); // Return 200 with the list of investment groups including IDs
        }

        [HttpPut("edit-group/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditInvestmentGroup(int id, [FromBody] string newGroupName)
        {
            var result = await _investmentGroupService.EditInvestmentGroupAsync(id, newGroupName);
            if (!result.IsSucceeded)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpDelete("delete-group/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteInvestmentGroup(int id)
        {
            var result = _investmentGroupService.DeleteInvestmentGroup(id);
            if (!result.IsSucceeded)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPost("join-group")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> JoinInvestmentGroup([FromBody] string groupName)
        {
            var result = await _investmentGroupService.JoinInvestmentGroupAsync(groupName, User);
            if (!result.IsSucceeded)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpGet("with-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetInvestmentGroupsWithUsers()
        {
            var groups = await _userInvestmentGroupService.GetInvestmentGroupsWithUsersAsync();
            return Ok(groups);
        }

        [HttpPost("{groupId}/leave")]
        [Authorize] // Ensure that the user is authenticated
        public async Task<IActionResult> LeaveInvestmentGroup(int groupId)
        {
            var result = await _userInvestmentGroupService.LeaveInvestmentGroupAsync(groupId, User);
            if (result.IsSucceeded)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }



    }
}

