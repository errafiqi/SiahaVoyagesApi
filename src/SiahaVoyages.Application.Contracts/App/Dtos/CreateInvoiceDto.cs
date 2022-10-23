using System;

namespace SiahaVoyages.App.Dtos
{
    public class CreateInvoiceDto
    {
        public Guid ClientId { get; set; }

        public DateTime Date { get; set; }

        public DateTime Mois { get; set; }

        public string Reference { get; set; }
    }
}
