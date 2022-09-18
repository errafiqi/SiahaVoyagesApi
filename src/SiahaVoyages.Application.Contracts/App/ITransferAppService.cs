using SiahaVoyages.App.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SiahaVoyages.App
{
    public interface ITransferAppService : IApplicationService
    {
        Task<TransferDto> GetAsync(Guid id);

        Task<PagedResultDto<TransferDto>> GetListAsync(GetTransferListDto input);

        Task<TransferDto> CreateAsync(CreateTransferDto input);

        Task<TransferDto> UpdateAsync(UpdateTransferDto input);

        Task DeleteAsync(Guid id);
    }
}
