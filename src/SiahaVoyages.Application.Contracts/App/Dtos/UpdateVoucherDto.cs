﻿using System;

namespace SiahaVoyages.App.Dtos
{
    public class UpdateVoucherDto
    {
        public Guid TransferId { get; set; }

        public DateTime Date { get; set; }

        public string Reference { get; set; }
    }
}
