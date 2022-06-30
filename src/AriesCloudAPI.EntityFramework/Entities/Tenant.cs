using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AriesCloudAPI.EntityFramework.Entities
{
    public abstract class BaseEntity<TKey>
    {
        [Required]
        [Key]
        public TKey Id { get; set; }
    } 

    public class Tenant : BaseEntity<Guid>
    {
        [Required]
        public string Aries_TenantId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string AccessToken { get; set; }

        //public string ImageUrl { get; set; }

        [Required]
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Updated { get; set; }

        // public virtual ICollection<Connection> Connections { get; set; }
        // public virtual ICollection<Schema> Schemas { get; set; }
        // public virtual ICollection<Role> Roles { get; set; }
    }

    public class Connection : BaseEntity<Guid>
    {
        public string Aries_ConnectionId { get; set; }

        [Required]
        public Guid? FkTenant1Id { get; set; }

        [ForeignKey("FkTenant1Id")]
        public virtual Tenant Tenant1 { get; set; }

        [Required]
        public Guid Tenant2Id { get; set; }

        //[ForeignKey("FkTenant2Id")]
        //public virtual Tenant Tenant2 { get; set; }

    }

    public class TenantSchema : BaseEntity<Guid>
    {
        [Required]
        public Guid? FkTenantId { get; set; }

        [ForeignKey("FkTenantId")]
        public virtual Tenant Tenant { get; set; }

        [Required]
        public Guid? FkSchemaId { get; set; }

        [ForeignKey("FkSchemaId")]
        public virtual Schema Schema { get; set; }

        public string Aries_CredentialDefinitionId { get; set; }
    }

    public class Schema : BaseEntity<Guid>
    { 
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Aries_SchemaID { get; set; }
    }

    public class Role : BaseEntity<Guid>
    {

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }

    //public class Invitation
    //{
    //    public Guid Id { get; set; }

    //    public Guid TenantId { get; set; }

    //    public string Data { get; set; }
    //}
}