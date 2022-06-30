using System;
using System.Threading.Tasks;
using AriesCloudAPI.Configuration.DependencyInjection.Options; 
using AriesCloudAPI.EntityFramework.Extensions;
using AriesCloudAPI.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AriesCloudAPI.EntityFramework.Contexts
{
    /// <summary>
    /// DbContext for the IdentityServer operational data.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// <seealso cref="AriesCloudAPI.EntityFramework.Interfaces.IPersistedStateDbContext" />
    public class PersistedStateDbContext : PersistedStateDbContext<PersistedStateDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersistedStateDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="storeOptions">The store options.</param>
        /// <exception cref="ArgumentNullException">storeOptions</exception>
        public PersistedStateDbContext(DbContextOptions<PersistedStateDbContext> options, OperationalStoreOptions storeOptions)
            : base(options, storeOptions)
        {
        }
    }

    /// <summary>
    /// DbContext for the IdentityServer operational data.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// <seealso cref="AriesCloudAPI.EntityFramework.Interfaces.IPersistedStateDbContext" />
    public class PersistedStateDbContext<TContext> : DbContext, IPersistedStateDbContext
        where TContext : DbContext, IPersistedStateDbContext
    {
        private readonly OperationalStoreOptions storeOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistedStateDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="storeOptions">The store options.</param>
        /// <exception cref="ArgumentNullException">storeOptions</exception>
        public PersistedStateDbContext(DbContextOptions options, OperationalStoreOptions storeOptions)
            : base(options)
        {
            if (storeOptions == null) throw new ArgumentNullException(nameof(storeOptions));
            this.storeOptions = storeOptions;
        }

        public DbSet<Entities.Tenant> Tenants { get; set; }
        public DbSet<Entities.Connection> Connections { get; set; }
        public DbSet<Entities.TenantSchema> TenantSchemas { get; set; }
        public DbSet<Entities.Schema> Schemas { get; set; }
        public DbSet<Entities.Role> Roles { get; set; } 


        /// <summary>
        /// Gets or sets the persisted tenants.
        /// </summary>
        /// <value>
        /// The device codes.
        /// </value>
        //public DbSet<Entities.PersistedTenant> PersistedTenants { get; set; }

        /// <summary>
        /// Gets or sets the persisted state.
        /// </summary>
        /// <value>
        /// The device codes.
        /// </value>
        public DbSet<Entities.PersistedState> PersistedStates { get; set; }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        public virtual Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigurePersistedGrantContext(storeOptions);

            base.OnModelCreating(modelBuilder);
        }

        // apply db migrations 
        public void ApplyDbMigrations()
        {
            Database.Migrate();
        }
    }
}