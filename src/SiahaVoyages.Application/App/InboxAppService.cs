using SiahaVoyages.App.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SiahaVoyages.App
{
    public class InboxAppService : SiahaVoyagesAppService, IInboxAppService
    {
        private readonly IRepository<Message, Guid> _messageRepository;

        public InboxAppService(IRepository<Message, Guid> messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<MessageDto> CreateAsync(CreateUpdateMessageDto input)
        {
            try
            {
                var message = ObjectMapper.Map<CreateUpdateMessageDto, Message>(input);
                var result = await _messageRepository.InsertAsync(message);
                return ObjectMapper.Map<Message, MessageDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
