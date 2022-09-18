using System;

namespace SiahaVoyages.App.Dtos
{
    public class CreateInvoiceDto
    {
        public Guid TransferId { get; set; }

        public byte[] File { get; set; }
    }
}
