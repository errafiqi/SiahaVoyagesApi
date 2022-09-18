using Volo.Abp.Application.Dtos;

namespace SiahaVoyages.App.Dtos
{
    public class GetTransferListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
