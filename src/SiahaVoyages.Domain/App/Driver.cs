using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace SiahaVoyages.App
{
    [Table(SiahaVoyagesConsts.DbTablePrefix + "Driver")]
    public class Driver : AuditedAggregateRoot<Guid>
    {
        [ForeignKey("UserId")]
        public Guid? UserId { get; set; }
        public IdentityUser User { get; set; }

        public bool Available { get; set; } = true;
    }
}
