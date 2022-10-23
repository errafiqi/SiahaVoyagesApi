using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace SiahaVoyages.App.Dtos
{
    public class MessageDto : FullAuditedEntityDto<Guid>
    {
        public Guid SenderId { get; set; }
        public IdentityUserDto Sender { get; set; }

        public Guid RecipientId { get; set; }
        public IdentityUserDto Recipient { get; set; }

        public string MessageContent { get; set; }

        public bool Read { get; set; }

        public bool Marked { get; set; }

        public bool Stared { get; set; }

        public bool Archived { get; set; }
    }
}
