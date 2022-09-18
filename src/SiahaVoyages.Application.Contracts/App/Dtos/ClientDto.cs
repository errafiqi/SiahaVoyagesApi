using System;
using Volo.Abp.Application.Dtos;

namespace SiahaVoyages.App.Dtos
{
    public class ClientDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
