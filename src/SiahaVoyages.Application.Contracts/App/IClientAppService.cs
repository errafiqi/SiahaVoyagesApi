using SiahaVoyages.App.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SiahaVoyages.App
{
    public interface IClientAppService : IApplicationService
    {
        Task<ClientDto> GetAsync(Guid id);

        Task<PagedResultDto<ClientDto>> GetListAsync(GetClientListDto input);

        Task<ClientDto> CreateAsync(CreateClientDto input);

        Task<ClientDto> UpdateAsync(Guid ClientId, UpdateClientDto input);

        Task DeleteAsync(Guid id);

        Task<ClientDto> EditPassword(Guid clientId, string newPassword);

        Task ChangeLogo(ChangeLogoDto input);
    }
}
