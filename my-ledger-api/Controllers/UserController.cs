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
        ///     [
        ///       {
        ///         "id": "42581618-6f25-45d9-8d5b-40af9be6278b",
        ///         "firstName": "Julio Nicolas",
        ///         "lastName": "Flores Rojas",
        ///         "email": "nico_fr1110@hotmail.com",
        ///         "password": null,
        ///         "roleId": "e0b5d470-e47e-4c7e-aaa8-b7c8cbebc4a2",
        ///         "isDeleted": false,
        ///         "role": {
        ///           "id": "e0b5d470-e47e-4c7e-aaa8-b7c8cbebc4a2",
        ///           "name": "Administrator",
        ///           "users": null
        ///         },
        ///         "transactions": []
        ///       }
        ///     ]
        /// </remarks>
        /// <response code="200">All Users were returned succesfully</response>
        /// <response code="500">No Users were found</response>
        //[Authorize(Roles = "Administrator")]
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
        ///     {
        ///        "id": "42581618-6f25-45d9-8d5b-40af9be6278b",
        ///        "firstName": "Julio Nicolas",
        ///        "lastName": "Flores Rojas",
        ///        "email": "nico_fr1110@hotmail.com",
        ///        "password": null,
        ///        "roleId": "e0b5d470-e47e-4c7e-aaa8-b7c8cbebc4a2",
        ///        "isDeleted": false,
        ///        "role": {
        ///          "id": "e0b5d470-e47e-4c7e-aaa8-b7c8cbebc4a2",
        ///          "name": "Administrator",
        ///          "users": null
        ///        },
        ///        "transactions": []
        ///     }
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
        ///     {
        ///       "firstName": "Julio Nicolas",
        ///       "lastName": "Flores Rojas",
        ///       "email": "nico_fr1110@hotmail.com",
        ///       "password": "password123",
        ///       "roleId": "e0b5d470-e47e-4c7e-aaa8-b7c8cbebc4a2",
        ///       "isDeleted": false,
        ///       "transactions": []
        ///     }
        /// </remarks>
        /// <param name="User"></param>
        /// <response code="200">The User was created succesfully</response>
        /// <response code="500">User could not be created</response>
        //[Authorize(Roles = "Administrator")]
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
        ///     {
        ///       "firstName": "Julio Nicolas",
        ///       "lastName": "Flores Rojas",
        ///       "email": "nico_fr1110@hotmail.com",
        ///       "roleId": "e0b5d470-e47e-4c7e-aaa8-b7c8cbebc4a2",
        ///       "isDeleted": false,
        ///       "role": {
        ///         "id": "e0b5d470-e47e-4c7e-aaa8-b7c8cbebc4a2",
        ///         "name": "Administrator",
        ///         "users": null
        ///       },
        ///       "transactions": []
        ///     }
        /// </remarks>
        /// <param name="User"></param>
        /// <param name="id"></param>
        /// <response code="200">The User was updated succesfully</response>
        /// <response code="500">User could not be updated</response>
        //[Authorize(Roles = "Administrator")]
        [HttpPut]
        [Route("{id}")]
        public UserDTO UpdateUser(UserDTO User, Guid id)
        {
            return _userManager.Update(User, id);
        }

        /// <summary>
        /// Update a User's password by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///         "CurrentPassword": "password123",
        ///         "NewPassword": "Pr0m3th3u$$"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="pwdUpdate"></param>
        /// <response code="200">The User's password was updated successfully</response>
        /// <response code="404">User's password could not be updated because user wasn't found</response>
        /// <response code="400">Current password is incorrect</response>
        /// <response code="422">New password cannot be the same as the previous one</response>
        [HttpPut]
        [Route("{id}/password")]
        public UserDTO UpdateUsersPassword(Guid id, UserUpdatePasswordDTO pwdUpdate)
        {
            return _userManager.UpdatePassword(id, pwdUpdate);
        }

        /// <summary>
        /// Soft Deletes a User by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///  
        ///     {
        ///        "id": "42581618-6f25-45d9-8d5b-40af9be6278b",
        ///        "firstName": "Julio Nicolas",
        ///        "lastName": "Flores Rojas",
        ///        "email": "nico_fr1110@hotmail.com",
        ///        "password": null,
        ///        "roleId": "e0b5d470-e47e-4c7e-aaa8-b7c8cbebc4a2",
        ///        "isDeleted": false,
        ///        "role": {
        ///          "id": "e0b5d470-e47e-4c7e-aaa8-b7c8cbebc4a2",
        ///          "name": "Administrator",
        ///          "users": null
        ///        },
        ///        "transactions": []
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="200">The User was deleted succesfully</response>
        /// <response code="500">User could not be deleted</response>
        //[Authorize(Roles = "Administrator")]
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
