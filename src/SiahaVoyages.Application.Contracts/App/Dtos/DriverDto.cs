using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace SiahaVoyages.App.Dtos
{
    public class DriverDto : AuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public IdentityUserDto User { get; set; }

        public string FullName
        {
            get
            {
                return User != null ? User.Name + " " + User.Surname : "";
            }
        }

        public bool Available { get; set; }
    }
}
