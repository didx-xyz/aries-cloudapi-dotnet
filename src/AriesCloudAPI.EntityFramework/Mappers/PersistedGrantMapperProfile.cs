using AriesCloudAPI.Core.Models;
using AutoMapper;

namespace AriesCloudAPI.EntityFramework.Mappers
{
    /// <summary>
    /// Defines entity/model mapping for persisted grants.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class PersistedGrantMapperProfile:Profile
    {
        /// <summary>
        /// <see cref="PersistedGrantMapperProfile">
        /// </see>
        /// </summary>
        public PersistedGrantMapperProfile()
        {
            CreateMap<Entities.PersistedGrant, PersistedInvitation>(MemberList.Destination)
                .ReverseMap();
        }
    }
}
