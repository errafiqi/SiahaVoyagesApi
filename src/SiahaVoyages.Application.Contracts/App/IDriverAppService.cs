using SiahaVoyages.App.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SiahaVoyages.App
{
    public interface IDriverAppService : IApplicationService
    {
        Task<DriverDto> GetAsync(Guid id);

        Task<PagedResultDto<DriverDto>> GetListAsync(GetDriverListDto input);

        Task<DriverDto> CreateAsync(CreateDriverDto input);

        Task<DriverDto> UpdateAsync(UpdateDriverDto input);

        Task DeleteAsync(Guid id);
    }
}
