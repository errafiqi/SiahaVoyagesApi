using Volo.Abp.Application.Dtos;

namespace SiahaVoyages.App.Dtos
{
    public class GetClientListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
