using System;
using System.Threading.Tasks;
using AriesCloudAPI.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace AriesCloudAPI.EntityFramework.Interfaces
{
    /// <summary>
    /// Abstraction for the operational data context.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IPersistedStateDbContext : IDisposable
    {
        /// <summary>
        /// Gets or sets the persisted grants.
        /// </summary>
        /// <value>
        /// The persisted grants.
        /// </value>
        //DbSet<PersistedGrant> PersistedGrants { get; set; }

        ///// <summary>
        ///// Gets or sets the persisted tenants.
        ///// </summary>
        ///// <value>
        ///// The persisted grants.
        ///// </value>
        //DbSet<PersistedTenant> PersistedTenants { get; set; }

        ///// <summary>
        ///// Gets or sets the persisted invitations.
        ///// </summary>
        ///// <value>
        ///// The persisted invitations.
        ///// </value>
        //DbSet<PersistedState> PersistedInvitations { get; set; }

        /// <summary>
        /// Gets or sets the persisted state.
        /// </summary>
        /// <value>
        /// The persisted state.
        /// </value>
        DbSet<PersistedState> PersistedStates { get; set; }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        void ApplyDbMigrations();
    }
}