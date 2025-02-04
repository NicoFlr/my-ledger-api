using Data.Exceptions;
using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.DTOModels;
using Services.Managers.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers.Transaction
{
    public class TransactionManager : ITransactionManager
    {
        private readonly UnitOfWork _unitOfWork;

        public TransactionManager(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TransactionDTO Create(TransactionDTO newTransaction)
        {
            try
            {
                Data.Models.Transaction transaction = new Data.Models.Transaction();
                transaction = DTOUtil.MapTransactionDTO(newTransaction);
                _unitOfWork.TransactionRepository.Add(transaction);
                _unitOfWork.Save();
                TransactionDTO transactionDTO = DTOUtil.MapTransactionToDTO(transaction);
                return transactionDTO;
            }
            catch (SystemException)
            {
                throw new NoContentException("Transaction with specified Id was not found.");
            }
        }

        public TransactionDTO CreateForUser(TransactionDTO newTransaction,Guid userId)
        {
            try
            {
                Data.Models.Transaction transaction = new Data.Models.Transaction();
                transaction = DTOUtil.MapTransactionDTO(newTransaction);
                _unitOfWork.TransactionRepository.Add(transaction);
                _unitOfWork.Save();
                TransactionDTO transactionDTO = DTOUtil.MapTransactionToDTO(transaction);

                Data.Models.User? foundUser = _unitOfWork.GetContext().Users.Include(u=>u.Transactions).Where(a => a.Id == userId).FirstOrDefault();
                if (foundUser != null)
                {
                    foundUser.Transactions.Add(transaction);
                    _unitOfWork.UserRepository.Update(foundUser);
                    _unitOfWork.Save();
                }
                else
                {
                    throw new EntityNotFoundError("The user with the specified Id was not found.");
                }

                return transactionDTO;
            }
            catch (SystemException)
            {
                throw new NoContentException("Transaction with specified Id was not found.");
            }
        }

        public TransactionDTO Get(Guid Id)
        {
            try
            {
                Data.Models.Transaction? transaction = _unitOfWork.GetContext().Transactions.Include(t=>t.Category).Where(a => a.Id == Id).FirstOrDefault();
                if (transaction != null)
                {
                    TransactionDTO transactionDTO = DTOUtil.MapTransactionToDTO(transaction);
                    return transactionDTO;
                }
                else
                {
                    throw new EntityNotFoundError("The transaction with the specified Id was not found.");
                }
            }
            catch (SystemException)
            {
                throw new UnexpectedError("Unexpected error.");
            }
        }

        public List<TransactionDTO> GetAll()
        {
            try
            {
                List<TransactionDTO> transactionsDTOList = new List<TransactionDTO>();
                List<Data.Models.Transaction> transactionList = _unitOfWork.GetContext().Transactions.Include(t => t.Category).ToList();
                transactionsDTOList = DTOUtil.MapTransactionToDTOList(transactionList);
                return transactionsDTOList;
            }
            catch (SystemException)
            {
                throw new UnexpectedError("Couldnt retrieve transactions list.");
            }
        }

        public TransactionDTO Update(TransactionDTO transaction, Guid id)
        {
            try
            {
                Data.Models.Transaction? foundTransaction = _unitOfWork.GetContext().Transactions.Where(a => a.Id == id).FirstOrDefault();
                if (foundTransaction != null)
                {
                    _unitOfWork.TransactionRepository.Detach(foundTransaction);
                    Data.Models.Transaction updatedTransaction = DTOUtil.MapTransactionDTO(transaction);
                    updatedTransaction.Id = id;
                    _unitOfWork.TransactionRepository.Update(updatedTransaction);
                    _unitOfWork.Save();

                    TransactionDTO transactionDTO = DTOUtil.MapTransactionToDTO(updatedTransaction);
                    return transactionDTO;
                }
                else
                {
                    throw new EntityNotFoundError("The transaction with the specified Id was not found.");
                }
            }
            catch (SystemException)
            {
                throw new UnexpectedError("The transaction with the specified Id couldnt be updated.");
            }
        }

        public TransactionDTO Delete(Guid id)
        {
            try
            {
                Data.Models.Transaction? transactionToDelete = _unitOfWork.GetContext().Transactions.Where(a => a.Id == id).FirstOrDefault();
                if (transactionToDelete != null)
                {
                    TransactionDTO transactionDTO = DTOUtil.MapTransactionToDTO(transactionToDelete);
                    _unitOfWork.TransactionRepository.Delete(transactionToDelete);
                    _unitOfWork.Save();
                    return transactionDTO;
                }
                else
                {
                    throw new EntityNotFoundError("The transaction with the specified Id was not found.");
                }
            }
            catch (SystemException)
            {
                throw new UnexpectedError("The transaction with the specified Id couldnt be deleted.");
            }
        }
    }
}
