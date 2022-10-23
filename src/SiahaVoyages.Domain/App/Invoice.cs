using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace SiahaVoyages.App
{
    [Table(SiahaVoyagesConsts.DbTablePrefix + "Invoice")]
    public class Invoice : AuditedAggregateRoot<Guid>
    {
        [ForeignKey("ClientId")]
        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        public DateTime Date { get; set; }

        public string Reference { get; set; }

        public DateTime Mois { get; set; }

        public float Prix { get; set; }

        public byte[] File { get; set; }
    }
}
