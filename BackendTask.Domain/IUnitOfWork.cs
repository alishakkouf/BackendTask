using System.Data;

namespace BackendTask.Domain
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Begin transaction on database with specific or no isolation level
        /// (Default Sql Server Isolation level is READ COMMITTED)
        /// </summary>
        Task BeginTransactionAsync(IsolationLevel? isolationLevel = null);

        /// <summary>
        /// Commit transaction and save to database
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Roll back transaction and undo changes on database
        /// </summary>
        Task RollBackAsync();
    }
}

