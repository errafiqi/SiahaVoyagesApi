using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace SiahaVoyages.App.Dtos
{
    public class DriverDto : AuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public IdentityUserDto User { get; set; }

        public bool Available { get; set; }
    }
}
