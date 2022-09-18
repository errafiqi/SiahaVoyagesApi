using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace SiahaVoyages.App
{
    [Table(SiahaVoyagesConsts.DbTablePrefix + "Invoice")]
    public class Invoice : AuditedAggregateRoot<Guid>
    {
        [ForeignKey("TransferId")]
        public Guid TransferId { get; set; }
        public Transfer Transfer { get; set; }

        public byte[] File { get; set; }
    }
}
