using System;

namespace SiahaVoyages.App.Dtos
{
    public class EditProfileInfosDto
    {
        public Guid DriverId { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfilePicture { get; set; }
    }
}
