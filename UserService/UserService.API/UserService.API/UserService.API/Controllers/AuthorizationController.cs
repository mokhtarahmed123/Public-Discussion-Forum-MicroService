using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UserService.API.Bases;
using UserService.Application.Feature.Authorization.Command.Model;
using UserService.Application.Feature.Authorization.Query.Model;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuthorizationController : AppBaseController
    {
        [HttpPost("AssignRole/{UserId}/{RoleId}")]
        [SwaggerOperation(Summary = "Assigns a role to a user", Description = "Allows an administrator to assign a specific role to a user.")]
        [SwaggerResponse(200, "Role assigned to user successfully", type: typeof(string))]
        [SwaggerResponse(400, "Invalid user or role data")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden")]
        [SwaggerResponse(404, "User or role not found")]
        [SwaggerResponse(500, "An unexpected error occurred")]
        public async Task<IActionResult> AssignRole(
     [FromRoute] AssignRoleToUserCommand command)
        {
            var response = await Mediator.Send(command);

            return NewResult(response);
        }


        [HttpPost("RemoveRoleFromUser/{roleId}/{userId}")]

        [SwaggerOperation(Summary = "Removes a role from a user", Description = "Allows an administrator to remove a specific role from a user.")]
        [SwaggerResponse(200, "Role removed from user successfully", type: typeof(string))]
        [SwaggerResponse(400, "Invalid user or role data")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden")]
        [SwaggerResponse(404, "User or role not found")]
        [SwaggerResponse(500, "An unexpected error occurred")]
        public async Task<IActionResult> RemoveRoleFromUser(
   [FromRoute] Guid roleId,
   [FromRoute] Guid userId)
        {
            var command = new RemoveRoleFromUserCommand(roleId, userId);

            var response = await Mediator.Send(command);

            return NewResult(response);
        }


        [HttpGet("GetUserRoles/{userId}")]
        [SwaggerOperation(Summary = "Gets user roles", Description = "Retrieves all roles assigned to a specific user.")]
        [SwaggerResponse(200, "User roles retrieved successfully", type: typeof(IList<string>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden")]
        [SwaggerResponse(404, "User not found")]
        [SwaggerResponse(500, "An unexpected error occurred")]
        public async Task<IActionResult> GetUserRoles(
    [FromRoute] Guid userId)
        {
            var query = new GetUserRolesQuery(userId);

            var response = await Mediator.Send(query);

            return NewResult(response);
        }

        [HttpGet("UserIsInRole/{userId}/{roleId}")]
        [SwaggerOperation(Summary = "Checks whether a user has a role", Description = "Checks if a specific user is assigned to a specific role.")]
        [SwaggerResponse(200, "User role status retrieved successfully", type: typeof(bool))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden")]
        [SwaggerResponse(404, "User or role not found")]
        [SwaggerResponse(500, "An unexpected error occurred")]
        public async Task<IActionResult> UserIsInRole(
[FromRoute] Guid userId,
[FromRoute] Guid roleId)
        {
            var query = new UserIsInRoleQuery(userId, roleId);

            var response = await Mediator.Send(query);

            return NewResult(response);
        }



    }
}
