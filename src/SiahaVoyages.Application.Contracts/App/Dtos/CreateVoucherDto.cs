using System;

namespace SiahaVoyages.App.Dtos
{
    public class CreateVoucherDto
    {
        public Guid TransferId { get; set; }

        public byte[] File { get; set; }
    }
}
