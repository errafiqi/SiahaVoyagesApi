using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace SiahaVoyages.App
{
    [Table(SiahaVoyagesConsts.DbTablePrefix + "Message")]
    public class Message : AuditedAggregateRoot<Guid>
    {
        public string MessageSubject { get; set; }

        public string MessageContent { get; set; }

        public bool Read { get; set; } = false;

        [ForeignKey("OriginMessageId")]
        public Guid? OriginMessageId { get; set; }
        public Message OriginMessage { get; set; }

        [ForeignKey("SenderId")]
        public Guid? SenderId { get; set; }
        public IdentityUser Sender { get; set; }

        public bool SenderMarked { get; set; } = false;

        public bool SenderStared { get; set; } = false;

        public bool SenderArchived { get; set; } = false;

        public bool SenderDeleted { get; set; } = false;

        [ForeignKey("RecipientId")]
        public Guid? RecipientId { get; set; }
        public IdentityUser Recipient { get; set; }

        public bool RecipientMarked { get; set; } = false;

        public bool RecipientStared { get; set; } = false;

        public bool RecipientArchived { get; set; } = false;

        public bool RecipientDeleted { get; set; } = false;
    }
}
