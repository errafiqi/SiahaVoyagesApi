﻿using System;
using Volo.Abp.Application.Dtos;

namespace SiahaVoyages.App.Dtos
{
    public class InvoiceDto : AuditedEntityDto<Guid>
    {
        public Guid TransferId { get; set; }
        public TransferDto Transfer  { get; set; }

        public DateTime Date { get; set; }

        public string Reference { get; set; }

        public byte[] File { get; set; }
    }
}
