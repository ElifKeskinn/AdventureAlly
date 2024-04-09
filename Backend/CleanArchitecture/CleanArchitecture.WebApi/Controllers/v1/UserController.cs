﻿using CleanArchitecture.Core.Features.Users.Commands.CreateProduct;
using CleanArchitecture.Core.Features.Users.Commands.DeleteProductById;
using CleanArchitecture.Core.Features.Users.Commands.UpdateProduct;
using CleanArchitecture.Core.Features.Users.Queries.GetAllProducts;
using CleanArchitecture.Core.Features.Users.Queries.GetProductById;
using CleanArchitecture.Core.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Get(int id)
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
        public async Task<IActionResult> Put(int id, UpdateUserCommand command)
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
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteUserByIdCommand { Id = id }));
        }
    }
}