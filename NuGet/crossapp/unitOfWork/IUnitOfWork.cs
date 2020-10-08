using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace crossapp.unitOfWork
{
    /// <summary>
    /// Unit of work
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Save pending changes
        /// </summary>
        Task SaveChanges();
        /// <summary>
        /// Add an action to execute before SaveChanges
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task AddPendingAction(Action action);


        //TODO: Se puede añazir un DiscardChanges, aunque es una práctica un tanto fea
    }
}
