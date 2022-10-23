using Volo.Abp.Application.Dtos;

namespace SiahaVoyages.App.Dtos
{
    public class GetMessageListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
