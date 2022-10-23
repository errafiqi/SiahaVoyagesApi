using System;
using Volo.Abp.Application.Dtos;

namespace SiahaVoyages.App.Dtos
{
    public class InvoiceDto : AuditedEntityDto<Guid>
    {
        public Guid ClientId { get; set; }
        public ClientDto Client { get; set; }

        public DateTime Date { get; set; }

        public DateTime Mois { get; set; }

        public string Reference { get; set; }

        public float Prix { get; set; }

        public byte[] File { get; set; }

        public ListResultDto<TransferDto> Transfers { get; set; }
    }
}
