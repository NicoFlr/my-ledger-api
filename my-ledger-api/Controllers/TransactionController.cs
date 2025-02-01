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
        /// 
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
