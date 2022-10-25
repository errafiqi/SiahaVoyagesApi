using SiahaVoyages.App.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace SiahaVoyages.App
{
    public interface IInboxAppService : IApplicationService
    {

        Task SendMessageAsync(CreateUpdateMessageDto input);

        Task<ListResultDto<IdentityUserDto>> GetBackOfficeUsersAsync();

        Task<ListResultDto<IdentityUserDto>> GetClientUsersAsync();

        Task<MessageDto> MarkAsync(Guid id);

        Task<MessageDto> ReadAsync(Guid id);

        Task<MessageDto> StarAsync(Guid id);

        Task<MessageDto> ArchiveAsync(Guid id);

        Task<MessageDto> DeleteAsync(Guid id);

        Task<MessageDto> UnmarkAsync(Guid id);

        Task<MessageDto> UnstarAsync(Guid id);

        Task<MessageDto> UnarchiveAsync(Guid id);

        Task<MessageDto> UndeleteAsync(Guid id);

        Task<MessageDto> GetAsync(Guid id);

        Task<MessageListDto> GetReceivedMessagesListAsync(GetMessageListDto input);

        Task<MessageListDto> GetSentMessagesListAsync(GetMessageListDto input);

        Task<MessageListDto> GetStaredMessagesListAsync(GetMessageListDto input);

        Task<MessageListDto> GetMarkedMessagesListAsync(GetMessageListDto input);

        Task<MessageListDto> GetArchivedMessagesListAsync(GetMessageListDto input);

        Task<MessageListDto> GetDeletedMessagesListAsync(GetMessageListDto input);
    }
}
