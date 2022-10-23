using SiahaVoyages.App.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SiahaVoyages.App
{
    public interface IMessageAppService : IApplicationService
    {
        Task<MessageDto> GetAsync(Guid id);

        Task<PagedResultDto<MessageDto>> GetListAsync(GetMessageListDto input);

        Task<MessageDto> CreateAsync(CreateUpdateMessageDto input);

        Task DeleteAsync(Guid id);

        Task<MessageDto> MarkAsync(Guid id);

        Task<MessageDto> ReadAsync(Guid id);

        Task<MessageDto> StarAsync(Guid id);

        Task<MessageDto> ArchiveAsync(Guid id);
    }
}
