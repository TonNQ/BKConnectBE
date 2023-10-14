using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Model.Dtos.Parameters;
using BKConnectBE.Service.Relationships;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Controllers.Relationships
{
    [CustomAuthorize]
    [ApiController]
    [Route("relationships")]
    public class RelationshipController : ControllerBase
    {
        private readonly IRelationshipService _relationshipService;

        public RelationshipController(IRelationshipService relationshipService)
        {
            _relationshipService = relationshipService;
        }

        [HttpGet("getFriends")]
        public async Task<ActionResult<Responses>> GetListOfFriendsOfUser()
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var listOfFriends = await _relationshipService.GetListOfFriendsByUserId(userId);
                    return this.Success(listOfFriends, MsgNo.SUCCESS_GET_FRIENDS_OF_USER);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("searchFriends")]
        public async Task<ActionResult<Responses>> SearchListOfFriendsOfUser(SearchKeyCondition searchKeyCondition)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var listOfFriends = await _relationshipService.SearchListOfFriendsByUserId(userId, searchKeyCondition.SearchKey);
                    return this.Success(listOfFriends, MsgNo.SUCCESS_GET_FRIENDS_OF_USER);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }
    }
}