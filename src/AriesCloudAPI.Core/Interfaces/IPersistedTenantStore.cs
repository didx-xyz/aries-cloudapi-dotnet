using AriesCloudAPI.Core.Models; 
using System.Threading.Tasks;

namespace AriesCloudAPI.Core.Interfaces
{
    /// <summary>
    /// Interface for persisting any type of Tenant.
    /// </summary>
    public interface IPersistedTenantStore
    {
        /// <summary>
        /// Stores the Tenant.
        /// </summary>
        /// <param name="Tenant">The Tenant.</param>
        /// <returns></returns>
        Task StoreAsync(PersistedTenant Tenant);

        /// <summary>
        /// Gets the Tenant.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<PersistedTenant> GetAsync(string key);

        /// <summary>
        /// Gets all Tenants based on the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        //Task<IEnumerable<PersistedTenant>> GetAllAsync(PersistedTenantFilter filter);

        /// <summary>
        /// Removes the Tenant by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task RemoveAsync(string key);

        /// <summary>
        /// Removes all Tenants based on the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        //Task RemoveAllAsync(PersistedTenantFilter filter);
    }
}