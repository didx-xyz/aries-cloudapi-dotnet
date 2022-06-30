using System;

namespace AriesCloudAPI.Core.Models
{
    /// <summary>
    /// A model for a persisted tenant
    /// </summary>
    public class PersistedTenant
    {
        public string Key { get; set; }
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public string AccessToken { get; set; }
        public string ImageUrl { get; set; }
        public string Created { get; set; }
        public string Updated { get; set; }
    }
}