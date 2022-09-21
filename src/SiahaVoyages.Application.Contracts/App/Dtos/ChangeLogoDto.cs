using System;
using System.Collections.Generic;
using System.Text;

namespace SiahaVoyages.App.Dtos
{
    public class ChangeLogoDto
    {
        public Guid clientId { get; set; }

        public string base64Image { get; set; }
    }
}
