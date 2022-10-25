using System;
using Volo.Abp.Identity;

namespace SiahaVoyages.App.Dtos
{
    public class CreateUpdateMessageDto
    {
        public string MessageSubject { get; set; }

        public string MessageContent { get; set; }

        public Guid? OriginMessageId { get; set; }

        public IdentityUserDto[] Recipients { get; set; }
    }
}
