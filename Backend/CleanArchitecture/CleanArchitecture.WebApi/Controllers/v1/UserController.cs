using CleanArchitecture.Core.Features.Users.Commands.CreateUser;
using CleanArchitecture.Core.Features.Users.Commands.DeleteUserById;
using CleanArchitecture.Core.Features.Users.Commands.UpdateUser;
using CleanArchitecture.Core.Features.Users.Queries.GetAllUsers;
using CleanArchitecture.Core.Features.Users.Queries.GetUserById;
using CleanArchitecture.Core.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class UserController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<IEnumerable<GetAllUsersViewModel>>))]
        public async Task<PagedResponse<IEnumerable<GetAllUsersViewModel>>> Get([FromQuery] GetAllUsersParameter filter)
        {
            return await Mediator.Send(new GetAllUsersQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber });
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(ObjectId id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CreateUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put([FromRoute] ObjectId id, [FromRoute] UpdateUserCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            return Ok(await Mediator.Send(new DeleteUserByIdCommand { Id = id }));
        }
    }
}
