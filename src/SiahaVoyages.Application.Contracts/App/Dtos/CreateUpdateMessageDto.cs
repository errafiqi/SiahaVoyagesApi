using System;

namespace SiahaVoyages.App.Dtos
{
    public class CreateUpdateMessageDto
    {
        public Guid SenderId { get; set; }

        public Guid RecipientId { get; set; }

        public string MessageContent { get; set; }

        public bool Read { get; set; } = false;

        public bool Marked { get; set; } = false;

        public bool Stared { get; set; } = false;

        public bool Archived { get; set; } = false;
    }
}
