using System;
using System.Linq.Expressions;
using CryptoAnalyzer.Data.Context;
using CryptoAnalyzer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoAnalyzer.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly CryptoAnalyzerDbContext _db;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(CryptoAnalyzerDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            _dbSet.Add(entity);
        }

        public void Delete(TEntity entity, bool softDelete = true)
        {

            if(softDelete)
            {
                entity.ModifiedDate = DateTime.Now;
                entity.IsDeleted = true;
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate is null ? _dbSet : _dbSet.Where(predicate);
        }

        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            _dbSet.Update(entity);


        }

        public async Task<TEntity> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null ? await Task.FromResult(_dbSet) : await Task.FromResult(_dbSet.Where(predicate).AsQueryable());
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<TransactionHistoryEntity> GetByInvestmentIdAsync(int investmentId)
        {
            // Use the DbSet to find the transaction history by investmentId
            return await _db.TransactionHistories
                .FirstOrDefaultAsync(th => th.InvestmentId == investmentId && !th.IsDeleted); // Add your soft delete filter here
        }

        public async Task DeleteByInvestmentIdAsync(int investmentId)
        {
            var transactionHistory = await _db.TransactionHistories
                .Where(th => th.InvestmentId == investmentId && !th.IsDeleted) // Ensure you're checking for non-deleted entries
                .FirstOrDefaultAsync();

            if (transactionHistory != null)
            {
                transactionHistory.IsDeleted = true; // Soft delete
                _db.TransactionHistories.Update(transactionHistory);
            }
        }

        public async Task<UserInvestmentGroupEntity?> GetByUserIdAndGroupIdAsync(int userId, int groupId)
        {
            return await _db.UserInvestmentGroups
                .FirstOrDefaultAsync(uig => uig.UserId == userId && uig.InvestmentGroupId == groupId);
        }
    }
}

