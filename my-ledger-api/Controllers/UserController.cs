using Microsoft.AspNetCore.Mvc;
using Services.DTOModels;
using Services.Managers.User;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Gets all Users
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        /// </remarks>
        /// <response code="200">All Users were returned succesfully</response>
        /// <response code="500">No Users were found</response>
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public List<UserDTO> getAllUsers()
        {
            return _userManager.GetAll();
        }

        /// <summary>
        /// Get a User by Id
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="200">The User was returned succesfully</response>
        /// <response code="500">User with specified Id was not found</response>
        [HttpGet]
        [Route("{id}")]
        public UserDTO GetUser(Guid id)
        {
            return _userManager.Get(id);
        }

        /// <summary>
        /// Creates a User
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="User"></param>
        /// <response code="200">The User was created succesfully</response>
        /// <response code="500">User could not be created</response>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public UserDTO CreateUser(UserDTO User)
        {
            return _userManager.Create(User);
        }

        /// <summary>
        /// Update a User by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="User"></param>
        /// <param name="id"></param>
        /// <response code="200">The User was updated succesfully</response>
        /// <response code="500">User could not be updated</response>
        [Authorize(Roles = "Administrator")]
        [HttpPut]
        [Route("{id}")]
        public UserDTO UpdateUser(UserDTO User, Guid id)
        {
            return _userManager.Update(User, id);
        }

        /// <summary>
        /// Soft Deletes a User by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="200">The User was deleted succesfully</response>
        /// <response code="500">User could not be deleted</response>
        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        [Route("{id}")]
        public UserDTO SoftDeleteUser(Guid id)
        {
            return _userManager.SoftDelete(id);
        }

        /// <summary>
        /// Logs in a user
        /// </summary>
        /// <response code="200">The User was logged in successfully</response>
        /// <response code="404">User was not found</response>
        /// <response code="401">Unauthorized user</response>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login([FromHeader] string auth)
        {
            string token = _userManager.Login(auth);
            token = "Bearer " + token;
            Response.Headers.Add("Authorization", token);
            return Ok();
        }
    }
}
