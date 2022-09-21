using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace SiahaVoyages.App
{
    [Table(SiahaVoyagesConsts.DbTablePrefix + "Bon")]
    public class Voucher : AuditedAggregateRoot<Guid>
    {
        [ForeignKey("TransferId")]
        public Guid TransferId { get; set; }
        public Transfer Transfer { get; set; }

        public DateTime Date { get; set; }

        public string Reference { get; set; }

        public byte[] File { get; set; }
    }
}
