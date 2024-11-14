using System;
using System.Linq.Expressions;
using CryptoAnalyzer.Data.Entities;

namespace CryptoAnalyzer.Data.Repositories
{
	public interface IRepository<TEntity> where TEntity:class
	{
		void Add(TEntity entity);

		void Delete(TEntity entity, bool softDelete = true);

		void Delete(int id);

		void Update(TEntity entity);

		TEntity GetById(int id);

		TEntity Get(Expression<Func<TEntity, bool>> predicate);

		IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null);

        Task<TEntity> GetByIdAsync(int id);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

		Task<TransactionHistoryEntity> GetByInvestmentIdAsync(int investmentId);

		Task DeleteByInvestmentIdAsync(int investmentId);

		Task<UserInvestmentGroupEntity?> GetByUserIdAndGroupIdAsync(int userId, int groupId);
    }
}

