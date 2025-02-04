using System;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Repositories
{
    public class UnitOfWork : System.IDisposable
    {
        private readonly MyLedgerDbContext _dbContext;

        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IRepository<User> _userRepository;

        private bool _disposed = false;

        public UnitOfWork(MyLedgerDbContext dbContext)
        {
            _dbContext = dbContext ?? new MyLedgerDbContext();

            _dbContext.Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded;

            _categoryRepository = new Repository<Category>(_dbContext);
            _roleRepository = new Repository<Role>(_dbContext);
            _transactionRepository = new Repository<Transaction>(_dbContext);
            _userRepository = new Repository<User>(_dbContext);
        }

        public void Save()
        {
            if (_dbContext.Database.AutoTransactionBehavior == AutoTransactionBehavior.Always
                && _dbContext.Database.CurrentTransaction == null)
            {
                try
                {
                    BeginTransaction();
                    _dbContext.SaveChanges();
                    CommitTransaction();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    RollBackTransaction();
                    throw;
                }
                catch (System.Exception)
                {
                    RollBackTransaction();
                    throw;
                }
            }
            else
            {
                _dbContext.SaveChanges();
            }
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }

        public void RollBackTransaction()
        {
            _dbContext.Database.RollbackTransaction();
        }

        public IDbContextTransaction ContextTransaction()
        {
            return _dbContext.Database.CurrentTransaction;
        }

        public IRepository<Category> CategoryRepository
        {
            get
            {
                return _categoryRepository;
            }
        }

        public IRepository<Role> RoleRepository
        {
            get
            {
                return _roleRepository;
            }
        }

        public IRepository<Transaction> TransactionRepository
        {
            get
            {
                return _transactionRepository;
            }
        }

        public IRepository<User> UserRepository
        {
            get
            {
                return _userRepository;
            }
        }

        public MyLedgerDbContext GetContext()
        {
            return _dbContext;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                _disposed = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(disposing: false);
        }
    }
}
