using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace SiahaVoyages.App.Dtos
{
    public class ClientDto : AuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public IdentityUserDto User { get; set; }

        public string Adresse { get; set; }

        public string ICE { get; set; }

        public string IF { get; set; }

        public string TP { get; set; }

        public string RC { get; set; }

        public string RIB { get; set; }

        public string Contact { get; set; }
    }
}
