using System.Threading.Tasks;

namespace AriesCloudAPI.Core.Interfaces
{
    /// <summary>
    /// Interface for persisting any type of State.
    /// </summary>
    public interface IStateStore
    {
        /// <summary>
        /// Stores the State.
        /// </summary>
        /// <param name="State">The State.</param>
        /// <returns></returns>
        Task StoreAsync(string key, object data);

        /// <summary>
        /// Gets the State.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Gets all States based on the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        //Task<IEnumerable<PersistedState>> GetAllAsync(PersistedStateFilter filter);

        /// <summary>
        /// Removes the State by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task RemoveAsync(string key);

        /// <summary>
        /// Removes all States based on the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        //Task RemoveAllAsync(PersistedStateFilter filter);
    }
}