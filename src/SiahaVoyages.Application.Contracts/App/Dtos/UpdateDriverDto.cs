using System;
using Volo.Abp.AuditLogging;
using Volo.Abp.Identity;

namespace SiahaVoyages.App.Dtos
{
    public class UpdateDriverDto
    {
        public Guid UserId { get; set; }
        public IdentityUserUpdateDto User { get; set; }

        public bool Available { get; set; }

        public string ProfilePicture { get; set; }
    }
}
