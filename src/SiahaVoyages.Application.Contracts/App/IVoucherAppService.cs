using SiahaVoyages.App.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SiahaVoyages.App
{
    public interface IVoucherAppService : IApplicationService
    {
        Task<VoucherDto> GetAsync(Guid id);

        Task<PagedResultDto<VoucherDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<VoucherDto> CreateAsync(CreateVoucherDto input);

        Task<VoucherDto> UpdateAsync(UpdateVoucherDto input);

        Task DeleteAsync(Guid id);
    }
}
