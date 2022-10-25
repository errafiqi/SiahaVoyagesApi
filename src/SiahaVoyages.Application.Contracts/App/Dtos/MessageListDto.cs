using Volo.Abp.Application.Dtos;

namespace SiahaVoyages.App.Dtos
{
    public class MessageListDto : PagedResultDto<MessageDto>
    {
        public bool IsReceivedList { get; set; } = false;
        public int ReceivedCount { get; set; }

        public bool IsStaredList { get; set; } = false;
        public int StaredCount { get; set; }

        public bool IsMarkedList { get; set; } = false;
        public int MarkedCount { get; set; }

        public bool IsSentList { get; set; } = false;
        public int SentCount { get; set; }

        public bool IsArchivedList { get; set; } = false;
        public int ArchivedCount { get; set; }

        public bool IsDeletedList { get; set; } = false;
        public int DeletedCount { get; set; }
    }
}
