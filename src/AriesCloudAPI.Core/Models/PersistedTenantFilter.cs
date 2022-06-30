﻿namespace AriesCloudAPI.Stores
{
    /// <summary>
    /// Represents a filter used when accessing the persisted Tenants store. 
    /// Setting multiple properties is interpreted as a logical 'AND' to further filter the query.
    /// At least one value must be supplied.
    /// </summary>
    public class PersistedTenantFilter
    {
        /// <summary>
        /// Subject id of the user.
        /// </summary>
        public string SubjectId { get; set; }
        
        /// <summary>
        /// Session id used for the Tenant.
        /// </summary>
        public string SessionId { get; set; }
        
        /// <summary>
        /// Client id the Tenant was issued to.
        /// </summary>
        public string ClientId { get; set; }
        
        /// <summary>
        /// The type of Tenant.
        /// </summary>
        public string Type { get; set; }
    }
}