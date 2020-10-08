using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace crossapp.repository
{
    /// <summary>
    /// Repository pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> : IDisposable
    {
        /// <summary>
        /// Get a specific entity
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <returns></returns>
        Task<T> Get<TKey>(TKey key);
        /// <summary>
        /// Gets elements from repository
        /// </summary>
        /// <param name="expression">Filter</param>
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> expression = null);
        /// <summary>
        /// Add an item
        /// </summary>
        /// <param name="item">item to add</param>
        Task Add(T item);
        /// <summary>
        /// Update an entity
        /// </summary>
        /// <returns></returns>
        Task Update(T obj);
        /// <summary>
        /// Removes items from repository
        /// </summary>
        Task Remove(T obj);

        //TODO: Se puede añadir un update partial y un delete por ID, aunque sería un antipatrón pero ganamos eficiencia
    }
}
