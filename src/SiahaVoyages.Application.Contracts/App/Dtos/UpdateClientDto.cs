﻿using System;
using Volo.Abp.Identity;

namespace SiahaVoyages.App.Dtos
{
    public class UpdateClientDto
    {
        public Guid UserId { get; set; }
        public IdentityUserUpdateDto User { get; set; }

        public string Name { get; set; }

        public string Adresse { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ICE { get; set; }

        public string IF { get; set; }

        public string TP { get; set; }

        public string RC { get; set; }

        public string RIB { get; set; }

        public string Contact { get; set; }

        public byte[] Logo { get; set; }
    }
}
