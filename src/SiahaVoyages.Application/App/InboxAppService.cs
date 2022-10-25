using SiahaVoyages.App.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace SiahaVoyages.App
{
    public class InboxAppService : SiahaVoyagesAppService, IInboxAppService
    {
        private readonly IRepository<Message, Guid> _messageRepository;
        private readonly IRepository<IdentityUser, Guid> _userRepository;
        private readonly IRepository<IdentityRole, Guid> _roleRepository;

        IdentityUserManager UserManager { get; }


        public InboxAppService(IRepository<Message, Guid> messageRepository,
            IRepository<IdentityUser, Guid> userRepository,
            IRepository<IdentityRole, Guid> roleRepository,
            IdentityUserManager userManager)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            UserManager = userManager;
        }

        public async Task SendMessageAsync(CreateUpdateMessageDto input)
        {
            var message = ObjectMapper.Map<CreateUpdateMessageDto, Message>(input);
            foreach (var recipient in input.Recipients)
            {
                message.RecipientId = recipient.Id;
                message.SenderId = CurrentUser.Id;
                await _messageRepository.InsertAsync(message);
            }
        }

        public async Task<ListResultDto<IdentityUserDto>> GetBackOfficeUsersAsync()
        {
            var roleId = (await _roleRepository.GetAsync(r => r.Name.Equals("office"))).Id;
            var users = (await _userRepository.WithDetailsAsync(u => u.Roles)).ToList();
            var officeUsers = new List<IdentityUser>();
            foreach (var user in users)
            {
                if (user.Roles != null && user.Roles.Any() && user.IsInRole(roleId))
                {
                    officeUsers.Add(user);
                }
            }
            var readOnlyUsersList = ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(officeUsers).AsReadOnly();
            return new ListResultDto<IdentityUserDto>(readOnlyUsersList);
        }

        public async Task<ListResultDto<IdentityUserDto>> GetClientUsersAsync()
        {
            var roleId = (await _roleRepository.GetAsync(r => r.Name.Equals("client"))).Id;
            var users = (await _userRepository.WithDetailsAsync(u => u.Roles)).ToList();
            var clientUsers = new List<IdentityUser>();
            foreach (var user in users)
            {
                if (user.Roles != null && user.Roles.Any() && user.IsInRole(roleId))
                {
                    clientUsers.Add(user);
                }
            }
            var readOnlyUsersList = ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(clientUsers).AsReadOnly();
            return new ListResultDto<IdentityUserDto>(readOnlyUsersList);
        }

        public async Task<MessageDto> MarkAsync(Guid id)
        {
            var message = await _messageRepository.GetAsync(id);
            if (CurrentUser.Id == message.SenderId)
            {
                message.SenderMarked = true;
            }
            else if (CurrentUser.Id == message.RecipientId)
            {
                message.RecipientMarked = true;
            }
            var result = await _messageRepository.UpdateAsync(message);
            return ObjectMapper.Map<Message, MessageDto>(result);
        }

        public async Task<MessageDto> UnmarkAsync(Guid id)
        {
            var message = await _messageRepository.GetAsync(id);
            if (CurrentUser.Id == message.SenderId)
            {
                message.SenderMarked = false;
            }
            else if (CurrentUser.Id == message.RecipientId)
            {
                message.RecipientMarked = false;
            }
            var result = await _messageRepository.UpdateAsync(message);
            return ObjectMapper.Map<Message, MessageDto>(result);
        }

        public async Task<MessageDto> ReadAsync(Guid id)
        {
            var message = await _messageRepository.GetAsync(id);
            message.Read = true;
            var result = await _messageRepository.UpdateAsync(message);
            return ObjectMapper.Map<Message, MessageDto>(result);
        }

        public async Task<MessageDto> StarAsync(Guid id)
        {
            var message = await _messageRepository.GetAsync(id);
            if (CurrentUser.Id == message.SenderId)
            {
                message.SenderStared = true;
            }
            else if (CurrentUser.Id == message.RecipientId)
            {
                message.RecipientStared = true;
            }
            var result = await _messageRepository.UpdateAsync(message);
            return ObjectMapper.Map<Message, MessageDto>(result);
        }

        public async Task<MessageDto> UnstarAsync(Guid id)
        {
            var message = await _messageRepository.GetAsync(id);
            if (CurrentUser.Id == message.SenderId)
            {
                message.SenderStared = false;
            }
            else if (CurrentUser.Id == message.RecipientId)
            {
                message.RecipientStared = false;
            }
            var result = await _messageRepository.UpdateAsync(message);
            return ObjectMapper.Map<Message, MessageDto>(result);
        }

        public async Task<MessageDto> ArchiveAsync(Guid id)
        {
            var message = await _messageRepository.GetAsync(id);
            if (CurrentUser.Id == message.SenderId)
            {
                message.SenderArchived = true;
            }
            else if (CurrentUser.Id == message.RecipientId)
            {
                message.RecipientArchived = true;
            }
            var result = await _messageRepository.UpdateAsync(message);
            return ObjectMapper.Map<Message, MessageDto>(result);
        }

        public async Task<MessageDto> UnarchiveAsync(Guid id)
        {
            var message = await _messageRepository.GetAsync(id);
            if (CurrentUser.Id == message.SenderId)
            {
                message.SenderArchived = false;
            }
            else if (CurrentUser.Id == message.RecipientId)
            {
                message.RecipientArchived = false;
            }
            var result = await _messageRepository.UpdateAsync(message);
            return ObjectMapper.Map<Message, MessageDto>(result);
        }

        public async Task<MessageDto> DeleteAsync(Guid id)
        {
            var message = await _messageRepository.GetAsync(id);
            if (CurrentUser.Id == message.SenderId)
            {
                message.SenderDeleted = true;
            }
            else if (CurrentUser.Id == message.RecipientId)
            {
                message.RecipientDeleted = true;
            }
            var result = await _messageRepository.UpdateAsync(message);
            return ObjectMapper.Map<Message, MessageDto>(result);
        }

        public async Task<MessageDto> UndeleteAsync(Guid id)
        {
            var message = await _messageRepository.GetAsync(id);
            if (CurrentUser.Id == message.SenderId)
            {
                message.SenderDeleted = false;
            }
            else if (CurrentUser.Id == message.RecipientId)
            {
                message.RecipientDeleted = false;
            }
            var result = await _messageRepository.UpdateAsync(message);
            return ObjectMapper.Map<Message, MessageDto>(result);
        }

        public async Task<MessageDto> GetAsync(Guid id)
        {
            var message = (await _messageRepository.WithDetailsAsync(m => m.Sender, m => m.Recipient))
                .FirstOrDefault(m => m.Id == id);
            return ObjectMapper.Map<Message, MessageDto>(message);
        }

        public async Task<MessageListDto> GetReceivedMessagesListAsync(GetMessageListDto input)
        {
            var result = await GetFilledMessageListDto();
            result.IsReceivedList = true;

            var messagesQuery = (await _messageRepository.WithDetailsAsync(m => m.Sender, m => m.Recipient))
                .Where(m => m.RecipientId == CurrentUser.Id && !m.RecipientDeleted && !m.RecipientArchived);

            var messages = messagesQuery.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            result.Items = ObjectMapper.Map<List<Message>, List<MessageDto>>(messages).AsReadOnly();
            result.TotalCount = messagesQuery.Count();

            return result;
        }

        public async Task<MessageListDto> GetSentMessagesListAsync(GetMessageListDto input)
        {
            var result = await GetFilledMessageListDto();
            result.IsSentList = true;

            var messagesQuery = (await _messageRepository.WithDetailsAsync(m => m.Sender, m => m.Recipient))
                .Where(m => m.SenderId == CurrentUser.Id && !m.SenderDeleted && !m.SenderArchived);

            var messages = messagesQuery.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            result.Items = ObjectMapper.Map<List<Message>, List<MessageDto>>(messages).AsReadOnly();
            result.TotalCount = messagesQuery.Count();

            return result;
        }

        public async Task<MessageListDto> GetStaredMessagesListAsync(GetMessageListDto input)
        {
            var result = await GetFilledMessageListDto();
            result.IsStaredList = true;

            var messagesQuery = (await _messageRepository.WithDetailsAsync(m => m.Sender, m => m.Recipient))
                .Where(m => m.RecipientId == CurrentUser.Id || m.SenderId == CurrentUser.Id)
                .Where(m => m.RecipientId != CurrentUser.Id || !m.RecipientDeleted && !m.RecipientArchived && m.RecipientStared)
                .Where(m => m.SenderId != CurrentUser.Id || !m.SenderDeleted && !m.SenderArchived && m.SenderStared);

            var messages = messagesQuery.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            result.Items = ObjectMapper.Map<List<Message>, List<MessageDto>>(messages).AsReadOnly();
            result.TotalCount = messagesQuery.Count();

            return result;
        }

        public async Task<MessageListDto> GetMarkedMessagesListAsync(GetMessageListDto input)
        {
            var result = await GetFilledMessageListDto();
            result.IsMarkedList = true;

            var messagesQuery = (await _messageRepository.WithDetailsAsync(m => m.Sender, m => m.Recipient))
                .Where(m => m.RecipientId == CurrentUser.Id || m.SenderId == CurrentUser.Id)
                .Where(m => m.RecipientId != CurrentUser.Id || !m.RecipientDeleted && !m.RecipientArchived && m.RecipientMarked)
                .Where(m => m.SenderId != CurrentUser.Id || !m.SenderDeleted && !m.SenderArchived && m.SenderMarked);

            var messages = messagesQuery.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            result.Items = ObjectMapper.Map<List<Message>, List<MessageDto>>(messages).AsReadOnly();
            result.TotalCount = messagesQuery.Count();

            return result;
        }

        public async Task<MessageListDto> GetArchivedMessagesListAsync(GetMessageListDto input)
        {
            var result = await GetFilledMessageListDto();
            result.IsArchivedList = true;

            var messagesQuery = (await _messageRepository.WithDetailsAsync(m => m.Sender, m => m.Recipient))
                .Where(m => m.RecipientId == CurrentUser.Id || m.SenderId == CurrentUser.Id)
                .Where(m => m.RecipientId != CurrentUser.Id || m.RecipientArchived)
                .Where(m => m.SenderId != CurrentUser.Id || m.SenderArchived);

            var messages = messagesQuery.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            result.Items = ObjectMapper.Map<List<Message>, List<MessageDto>>(messages).AsReadOnly();
            result.TotalCount = messagesQuery.Count();

            return result;
        }

        public async Task<MessageListDto> GetDeletedMessagesListAsync(GetMessageListDto input)
        {
            var result = await GetFilledMessageListDto();
            result.IsDeletedList = true;

            var messagesQuery = (await _messageRepository.WithDetailsAsync(m => m.Sender, m => m.Recipient))
                .Where(m => m.RecipientId == CurrentUser.Id || m.SenderId == CurrentUser.Id)
                .Where(m => m.RecipientId != CurrentUser.Id || m.RecipientDeleted)
                .Where(m => m.SenderId != CurrentUser.Id || m.SenderDeleted);

            var messages = messagesQuery.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            result.Items = ObjectMapper.Map<List<Message>, List<MessageDto>>(messages).AsReadOnly();
            result.TotalCount = messagesQuery.Count();

            return result;
        }

        private async Task<MessageListDto> GetFilledMessageListDto()
        {
            var result = new MessageListDto();

            var messagesQuery = await _messageRepository.GetQueryableAsync();

            result.ReceivedCount = messagesQuery.Where(m => m.RecipientId == CurrentUser.Id && !m.RecipientDeleted && !m.RecipientArchived).Count();

            result.SentCount = messagesQuery.Where(m => m.SenderId == CurrentUser.Id && !m.SenderDeleted && !m.SenderArchived).Count();

            result.StaredCount = messagesQuery.Where(m => m.RecipientId == CurrentUser.Id || m.SenderId == CurrentUser.Id)
                    .Where(m => m.RecipientId != CurrentUser.Id || !m.RecipientDeleted && !m.RecipientArchived && m.RecipientStared)
                    .Where(m => m.SenderId != CurrentUser.Id || !m.SenderDeleted && !m.SenderArchived && m.SenderStared)
                    .Count();

            result.MarkedCount = messagesQuery.Where(m => m.RecipientId == CurrentUser.Id || m.SenderId == CurrentUser.Id)
                    .Where(m => m.RecipientId != CurrentUser.Id || !m.RecipientDeleted && !m.RecipientArchived && m.RecipientMarked)
                    .Where(m => m.SenderId != CurrentUser.Id || !m.SenderDeleted && !m.SenderArchived && m.SenderMarked)
                    .Count();

            result.ArchivedCount = messagesQuery.Where(m => m.RecipientId == CurrentUser.Id || m.SenderId == CurrentUser.Id)
                    .Where(m => m.RecipientId != CurrentUser.Id || m.RecipientArchived)
                    .Where(m => m.SenderId != CurrentUser.Id || m.SenderArchived)
                    .Count();

            result.DeletedCount = messagesQuery.Where(m => m.RecipientId == CurrentUser.Id || m.SenderId == CurrentUser.Id)
                    .Where(m => m.RecipientId != CurrentUser.Id || m.RecipientDeleted)
                    .Where(m => m.SenderId != CurrentUser.Id || m.SenderDeleted)
                    .Count();

            return result;
        }
    }
}
