using SiahaVoyages.App.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SiahaVoyages.App
{
    public class MessageAppService : SiahaVoyagesAppService, IMessageAppService
    {
        private readonly IRepository<Message, Guid> _messageRepository;

        public MessageAppService(IRepository<Message, Guid> messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<MessageDto> ArchiveAsync(Guid id)
        {
            var message = await _messageRepository.GetAsync(id);
            message.Archived = true;
            var result = await _messageRepository.UpdateAsync(message);
            return ObjectMapper.Map<Message, MessageDto>(result);
        }

        public async Task<MessageDto> CreateAsync(CreateUpdateMessageDto input)
        {
            var message = ObjectMapper.Map<CreateUpdateMessageDto, Message>(input);
            var result = await _messageRepository.InsertAsync(message);
            return ObjectMapper.Map<Message, MessageDto>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _messageRepository.DeleteAsync(id);
        }

        public async Task<MessageDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResultDto<MessageDto>> GetListAsync(GetMessageListDto input)
        {
            throw new NotImplementedException();
        }

        public async Task<MessageDto> MarkAsync(Guid id)
        {
            var message = await _messageRepository.GetAsync(id);
            message.Marked = true;
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
            message.Stared = true;
            var result = await _messageRepository.UpdateAsync(message);
            return ObjectMapper.Map<Message, MessageDto>(result);
        }
    }
}
