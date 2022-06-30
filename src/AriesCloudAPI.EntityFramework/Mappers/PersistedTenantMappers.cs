// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AutoMapper; 
using AriesCloudAPI.Core.Models;

namespace AriesCloudAPI.EntityFramework.Mappers
{
    /// <summary>
    /// Extension methods to map to/from entity/model for persisted tenants.
    /// </summary>
    public static class PersistedTenantMappers
    {
        static PersistedTenantMappers()
        {
            Mapper = new MapperConfiguration(cfg =>cfg.AddProfile<PersistedTenantMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static PersistedTenant ToModel(this Entities.PersistedTenant entity)
        {
            return entity == null ? null : Mapper.Map<PersistedTenant>(entity);
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Entities.PersistedTenant ToEntity(this PersistedTenant model)
        {
            return model == null ? null : Mapper.Map<Entities.PersistedTenant>(model);
        }

        /// <summary>
        /// Updates an entity from a model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="entity">The entity.</param>
        public static void UpdateEntity(this PersistedTenant model, Entities.PersistedTenant entity)
        {
            Mapper.Map(model, entity);
        }
    }
}