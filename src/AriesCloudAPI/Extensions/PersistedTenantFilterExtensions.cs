using AriesCloudAPI.Stores;
using System;

namespace AriesCloudAPI.Extensions
{
    /// <summary>
    /// Extensions for PersistedTenantFilter.
    /// </summary>
    public static class PersistedTenantFilterExtensions
    {
        /// <summary>
        /// Validates the PersistedTenantFilter and throws if invalid.
        /// </summary>
        /// <param name="filter"></param>
        public static void Validate(this PersistedTenantFilter filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            if (String.IsNullOrWhiteSpace(filter.ClientId) &&
                String.IsNullOrWhiteSpace(filter.SessionId) &&
                String.IsNullOrWhiteSpace(filter.SubjectId) &&
                String.IsNullOrWhiteSpace(filter.Type))
            {
                throw new ArgumentException("No filter values set.", nameof(filter));
            }
        }
    }
}