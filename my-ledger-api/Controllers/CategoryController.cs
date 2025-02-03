using Microsoft.AspNetCore.Mvc;
using Services.DTOModels;
using Services.Managers.Category;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;

        public CategoryController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        /// <summary>
        /// Gets all Categories
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     [
        ///       {
        ///         "id": "85ab9918-007b-4804-aeef-0e7f99d54a4c",
        ///         "name": "Transport",
        ///         "transactions": []
        ///       },
        ///       {
        ///         "id": "0beb373e-c5dc-4420-b0e0-152c70b90661",
        ///         "name": "Food",
        ///         "transactions": []
        ///       }
        ///     ]
        /// </remarks>
        /// <response code="200">All Categories were returned succesfully</response>
        /// <response code="500">No Categories were found</response>
        [HttpGet]
        public List<CategoryDTO> getAllCategories()
        {
            return _categoryManager.GetAll();
        }

        /// <summary>
        /// Get a Category by Id
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     {
        ///       "id": "85ab9918-007b-4804-aeef-0e7f99d54a4c",
        ///       "name": "Transport",
        ///       "transactions": []
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="200">The Category was returned succesfully</response>
        /// <response code="500">Category with specified Id was not found</response>
        [HttpGet]
        [Route("{id}")]
        public CategoryDTO GetCategory(Guid id)
        {
            return _categoryManager.Get(id);
        }

        /// <summary>
        /// Creates a Category
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "name": "Food"
        ///     }
        /// </remarks>
        /// <param name="Category"></param>
        /// <response code="200">The Category was created succesfully</response>
        /// <response code="500">Category could not be created</response>
        //[Authorize(Roles = "Administrator")]
        [HttpPost]
        public CategoryDTO CreateCategory(CategoryDTO Category)
        {
            return _categoryManager.Create(Category);
        }

        /// <summary>
        /// Update a Category by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "name": "Food"
        ///     }
        /// </remarks>
        /// <param name="Category"></param>
        /// <param name="id"></param>
        /// <response code="200">The Category was updated succesfully</response>
        /// <response code="500">Category could not be updated</response>
        //[Authorize(Roles = "Administrator")]
        [HttpPut]
        [Route("{id}")]
        public CategoryDTO UpdateCategory(CategoryDTO Category, Guid id)
        {
            return _categoryManager.Update(Category, id);
        }

        /// <summary>
        /// Delete a Category by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "id": "85ab9918-007b-4804-aeef-0e7f99d54a4c",
        ///       "name": "Transport",
        ///       "transactions": []
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="200">The Category was deleted succesfully</response>
        /// <response code="500">Category could not be deleted</response>
        //[Authorize(Roles = "Administrator")]
        [HttpDelete]
        [Route("{id}")]
        public CategoryDTO DeleteCategory(Guid id)
        {
            return _categoryManager.Delete(id);
        }
    }
}
