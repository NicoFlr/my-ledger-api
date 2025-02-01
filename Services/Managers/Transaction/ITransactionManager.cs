using Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers.Transaction
{
    public interface ITransactionManager
    {
        TransactionDTO Get(Guid Id);
        List<TransactionDTO> GetAll();
        TransactionDTO Create(TransactionDTO newTransaction);
        TransactionDTO Update(TransactionDTO transactionToUpdate, Guid id);
        TransactionDTO Delete(Guid id);
    }
}
