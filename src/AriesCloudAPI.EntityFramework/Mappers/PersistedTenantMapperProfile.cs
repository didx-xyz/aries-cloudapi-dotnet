using AriesCloudAPI.Core.Models;
using AutoMapper;

namespace AriesCloudAPI.EntityFramework.Mappers
{
    /// <summary>
    /// Defines entity/model mapping for persisted tenants.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class PersistedTenantMapperProfile:Profile
    {
        /// <summary>
        /// <see cref="PersistedTenantMapperProfile">
        /// </see>
        /// </summary>
        public PersistedTenantMapperProfile()
        {
            CreateMap<Entities.PersistedTenant, PersistedTenant>(MemberList.Destination)
                .ReverseMap();
        }
    }
}
