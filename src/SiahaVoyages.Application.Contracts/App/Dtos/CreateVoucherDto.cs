using System;

namespace SiahaVoyages.App.Dtos
{
    public class CreateVoucherDto
    {
        public Guid TransferId { get; set; }

        public DateTime Date { get; set; }

        public string Reference { get; set; }
    }
}
