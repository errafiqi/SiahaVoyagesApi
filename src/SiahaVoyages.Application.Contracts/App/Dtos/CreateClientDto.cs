namespace SiahaVoyages.App.Dtos
{
    public class CreateClientDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Surname { get; set; }

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
