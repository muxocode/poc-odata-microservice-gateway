using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace crossapp.rule
{
    /// <summary>
    /// Rule processor
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public interface IRuleProcessor<T> where T : class
    {
        /// <summary>
        /// Rules to check
        /// </summary>
        IEnumerable<IRule<T>> Rules { get; }
        /// <summary>
        /// Check rules
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        Task<bool> CheckRules(T entity);
    }
}
