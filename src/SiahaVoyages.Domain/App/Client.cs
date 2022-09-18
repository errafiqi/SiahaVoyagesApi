using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace SiahaVoyages.App
{
    [Table(SiahaVoyagesConsts.DbTablePrefix + "Client")]
    public class Client : AuditedAggregateRoot<Guid>
    {
        [ForeignKey("UserId")]
        public Guid? UserId { get; set; }
        public IdentityUser User { get; set; }

        public string Adresse { get; set; }

        public string ICE { get; set; }

        public string IF { get; set; }

        public string TP { get; set; }

        public string RC { get; set; }

        public string RIB { get; set; }

        public string Contact { get; set; }
    }
}
