using System;
namespace CryptoAnalyzer.Data.UnitOfWork
{
	public interface IUnitOfWork:IDisposable
	{
		Task<int> SaveChangesAsync();

		Task BeginTransaction();

		Task CommitTransaction();

		Task RollBackTransaction();
	}
}

