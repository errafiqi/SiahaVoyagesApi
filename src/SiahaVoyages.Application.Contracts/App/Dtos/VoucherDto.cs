using System;
using Volo.Abp.Application.Dtos;

namespace SiahaVoyages.App.Dtos
{
    public class VoucherDto : AuditedEntityDto<Guid>
    {
        public Guid TransferId { get; set; }
        public TransferDto Transfer  { get; set; }

        public byte[] File { get; set; }
    }
}
