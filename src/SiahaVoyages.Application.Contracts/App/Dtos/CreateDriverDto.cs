using System;
using Volo.Abp.Identity;

namespace SiahaVoyages.App.Dtos
{
    public class CreateDriverDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public bool Available { get; set; }
    }
}
