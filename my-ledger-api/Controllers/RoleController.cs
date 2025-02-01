using Microsoft.AspNetCore.Mvc;
using Services.DTOModels;
using Services.Managers.User;
using System.Collections.Generic;
using System;
using Services.Managers.Role;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleManager _roleManager;

        public RoleController(IRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Gets all Roles
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     [
        ///       {
        ///         "id": "b7a7c3d9-755e-4018-921e-1da37fa173ec",
        ///         "name": "User",
        ///         "users": null
        ///       },
        ///       {
        ///         "id": "e0b5d470-e47e-4c7e-aaa8-b7c8cbebc4a2",
        ///         "name": "Administrator",
        ///         "users": null
        ///       }
        ///     ]
        /// </remarks>
        /// <response code="200">All Roles were returned succesfully</response>
        /// <response code="500">No Roles were found</response>
        [HttpGet]
        public List<RoleDTO> getAllRoles()
        {
            return _roleManager.GetAll();
        }

        /// <summary>
        /// Get a Role by Id
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     {
        ///        "id": "b7a7c3d9-755e-4018-921e-1da37fa173ec",
        ///        "name": "User",
        ///        "users": null
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="200">The Role was returned succesfully</response>
        /// <response code="500">Role with specified Id was not found</response>
        [HttpGet]
        [Route("{id}")]
        public RoleDTO GetRole(Guid id)
        {
            return _roleManager.Get(id);
        }

        /// <summary>
        /// Creates a Role
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "name": "Supervisor"
        ///     }
        /// </remarks>
        /// <param name="Role"></param>
        /// <response code="200">The Role was created succesfully</response>
        /// <response code="500">Role could not be created</response>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public RoleDTO CreateRole(RoleDTO Role)
        {
            return _roleManager.Create(Role);
        }

        /// <summary>
        /// Update a Role by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "name": "User"
        ///     }
        /// </remarks>
        /// <param name="Role"></param>
        /// <param name="id"></param>
        /// <response code="200">The Role was updated succesfully</response>
        /// <response code="500">Role could not be updated</response>
        [Authorize(Roles = "Administrator")]
        [HttpPut]
        [Route("{id}")]
        public RoleDTO UpdateRole(RoleDTO Role, Guid id)
        {
            return _roleManager.Update(Role, id);
        }
    }
}
