using Volo.Abp.Application.Dtos;

namespace SiahaVoyages.App.Dtos
{
    public class GetDriverListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
