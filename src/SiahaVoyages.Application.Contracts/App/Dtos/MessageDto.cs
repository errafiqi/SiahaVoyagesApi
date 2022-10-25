using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace SiahaVoyages.App.Dtos
{
    public class MessageDto : AuditedEntityDto<Guid>
    {
        public string MessageSubject { get; set; }

        public string MessageContent { get; set; }

        public bool Read { get; set; }

        public Guid? OriginMessageId { get; set; }
        public MessageDto OriginMessage { get; set; }

        public Guid SenderId { get; set; }
        public IdentityUserDto Sender { get; set; }

        public bool SenderMarked { get; set; }

        public bool SenderStared { get; set; }

        public bool SenderArchived { get; set; }

        public bool SenderDeleted { get; set; }

        public Guid RecipientId { get; set; }
        public IdentityUserDto Recipient { get; set; }

        public bool RecipientMarked { get; set; }

        public bool RecipientStared { get; set; }

        public bool RecipientArchived { get; set; }

        public bool RecipientDeleted { get; set; }
    }
}
