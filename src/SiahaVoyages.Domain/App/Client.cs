using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace SiahaVoyages.App
{
    [Table(SiahaVoyagesConsts.DbTablePrefix + "Client")]
    public class Client : AuditedAggregateRoot<Guid>
    {
        [Required]
        public string Name { get; set; }
    }
}
