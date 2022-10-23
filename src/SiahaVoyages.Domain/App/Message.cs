using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace SiahaVoyages.App
{
    [Table(SiahaVoyagesConsts.DbTablePrefix + "Message")]
    public class Message : FullAuditedAggregateRoot<Guid>
    {
        [ForeignKey("SenderId")]
        public Guid? SenderId { get; set; }
        public IdentityUser Sender { get; set; }

        [ForeignKey("RecipientId")]
        public Guid? RecipientId { get; set; }
        public IdentityUser Recipient { get; set; }

        public string MessageContent { get; set; }

        public bool Read { get; set; }

        public bool Marked { get; set; }

        public bool Stared { get; set; }

        public bool Archived { get; set; }
    }
}
