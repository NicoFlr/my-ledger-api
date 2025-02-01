using Microsoft.AspNetCore.Mvc;
using Services.DTOModels;
using Services.Managers.User;
using System.Collections.Generic;
using System;
using Services.Managers.Transaction;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionManager _transactionManager;

        public TransactionController(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        /// <summary>
        /// Gets all Transactions
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     [
        ///       {
        ///         "id": "ce41629a-bd68-48d5-8238-3989f6b6e3c8",
        ///         "money": 55,
        ///         "dateTime": "2025-01-20T10:04:52.287-04:00",
        ///         "isBill": false,
        ///         "categoryId": "799b89f8-bf63-48ae-9221-2252ce4b54cb",
        ///         "category": {
        ///             "id": "799b89f8-bf63-48ae-9221-2252ce4b54cb",
        ///             "name": "Work",
        ///             "transactions": null
        ///         },
        ///         "users": []
        ///       },
        ///       {
        ///         "id": "10c15097-d8d5-4c2d-a544-c163fc785b86",
        ///         "money": 0.5,
        ///         "dateTime": "2025-01-22T11:36:41.468-04:00",
        ///         "isBill": true,
        ///         "categoryId": "85ab9918-007b-4804-aeef-0e7f99d54a4c",
        ///         "category": {
        ///             "id": "85ab9918-007b-4804-aeef-0e7f99d54a4c",
        ///             "name": "Transport",
        ///             "transactions": null
        ///         },
        ///         "users": []
        ///       }
        ///     ]
        /// </remarks>
        /// <response code="200">All Transactions were returned succesfully</response>
        /// <response code="500">No Transactions were found</response>
        [HttpGet]
        public List<TransactionDTO> getAllTransactions()
        {
            return _transactionManager.GetAll();
        }

        /// <summary>
        /// Get a Transaction by Id
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///     {
        ///       "id": "10c15097-d8d5-4c2d-a544-c163fc785b86",
        ///       "money": 0.5,
        ///       "dateTime": "2025-01-22T11:36:41.468-04:00",
        ///       "isBill": true,
        ///       "categoryId": "85ab9918-007b-4804-aeef-0e7f99d54a4c",
        ///       "category": {
        ///           "id": "85ab9918-007b-4804-aeef-0e7f99d54a4c",
        ///           "name": "Transport",
        ///           "transactions": null
        ///       },
        ///       "users": []
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="200">The Transaction was returned succesfully</response>
        /// <response code="500">Transaction with specified Id was not found</response>
        [HttpGet]
        [Route("{id}")]
        public TransactionDTO GetTransaction(Guid id)
        {
            return _transactionManager.Get(id);
        }

        /// <summary>
        /// Creates a Transaction
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///        "money": 0.5,
        ///        "dateTime": "2025-01-22T11:36:41.468-04:00",
        ///        "isBill": true,
        ///        "categoryId": "85ab9918-007b-4804-aeef-0e7f99d54a4c"
        ///     }
        /// </remarks>
        /// <param name="Transaction"></param>
        /// <response code="200">The Transaction was created succesfully</response>
        /// <response code="500">Transaction could not be created</response>
        [HttpPost]
        public TransactionDTO CreateTransaction(TransactionDTO Transaction)
        {
            return _transactionManager.Create(Transaction);
        }

        /// <summary>
        /// Update a Transaction by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///       "money": 0.5,
        ///       "dateTime": "2025-01-22T11:36:41.468-04:00",
        ///       "isBill": true,
        ///       "categoryId": "85ab9918-007b-4804-aeef-0e7f99d54a4c"
        ///     }
        /// </remarks>
        /// <param name="Transaction"></param>
        /// <param name="id"></param>
        /// <response code="200">The Transaction was updated succesfully</response>
        /// <response code="500">Transaction could not be updated</response>
        [HttpPut]
        [Route("{id}")]
        public TransactionDTO UpdateTransaction(TransactionDTO Transaction, Guid id)
        {
            return _transactionManager.Update(Transaction, id);
        }

        /// <summary>
        /// Delete a Transaction by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///       
        ///     {
        ///       "id": "10c15097-d8d5-4c2d-a544-c163fc785b86",
        ///       "money": 0.5,
        ///       "dateTime": "2025-01-22T11:36:41.468-04:00",
        ///       "isBill": true,
        ///       "categoryId": "85ab9918-007b-4804-aeef-0e7f99d54a4c",
        ///       "category": {
        ///           "id": "85ab9918-007b-4804-aeef-0e7f99d54a4c",
        ///           "name": "Transport",
        ///           "transactions": null
        ///       },
        ///       "users": []
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="200">The Transaction was deleted succesfully</response>
        /// <response code="500">Transaction could not be deleted</response>
        [HttpDelete]
        [Route("{id}")]
        public TransactionDTO DeleteTransaction(Guid id)
        {
            return _transactionManager.Delete(id);
        }
    }
}
