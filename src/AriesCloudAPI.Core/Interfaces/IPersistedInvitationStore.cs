using AriesCloudAPI.Core.Models; 
using System.Threading.Tasks;

namespace AriesCloudAPI.Core.Interfaces
{
    /// <summary>
    /// Interface for persisting any type of Invitation.
    /// </summary>
    public interface IPersistedInvitationStore
    {
        /// <summary>
        /// Stores the Invitation.
        /// </summary>
        /// <param name="Invitation">The Invitation.</param>
        /// <returns></returns>
        Task StoreAsync(PersistedInvitation Invitation);

        /// <summary>
        /// Gets the Invitation.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<PersistedInvitation> GetAsync(string key);

        /// <summary>
        /// Gets all Invitations based on the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        //Task<IEnumerable<PersistedInvitation>> GetAllAsync(PersistedInvitationFilter filter);

        /// <summary>
        /// Removes the Invitation by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task RemoveAsync(string key);

        /// <summary>
        /// Removes all Invitations based on the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        //Task RemoveAllAsync(PersistedInvitationFilter filter);
    }
}