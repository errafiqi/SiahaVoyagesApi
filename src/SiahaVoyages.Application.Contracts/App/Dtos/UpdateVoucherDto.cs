using System;

namespace SiahaVoyages.App.Dtos
{
    public class UpdateVoucherDto
    {
        public Guid TransferId { get; set; }

        public byte[] File { get; set; }
    }
}
