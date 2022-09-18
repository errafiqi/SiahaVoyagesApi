using SiahaVoyages.App.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace SiahaVoyages.App
{
    public class ClientAppService : SiahaVoyagesAppService, IClientAppService
    {
        IRepository<Client, Guid> _clientRepository;

        public ClientAppService(IRepository<Client, Guid> ClientRepository)
        {
            _clientRepository = ClientRepository;
        }

        public async Task<ClientDto> GetAsync(Guid id)
        {
            var client = await _clientRepository.GetAsync(id);
            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public async Task<PagedResultDto<ClientDto>> GetListAsync(GetClientListDto input)
        {
            var query = await _clientRepository.GetQueryableAsync();

            var clients = query.WhereIf(!string.IsNullOrEmpty(input.Filter), d => d.Name.Contains(input.Filter))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderBy(d => d.LastModificationTime != null ? d.LastModificationTime : d.CreationTime)
                .ToList();

            var totalCount = clients.Any() ? clients.Count() : 0;

            return new PagedResultDto<ClientDto>(
                totalCount,
                ObjectMapper.Map<List<Client>, List<ClientDto>>(clients)
            );
        }

        public async Task<ClientDto> CreateAsync(CreateClientDto input)
        {
            var client = ObjectMapper.Map<CreateClientDto, Client>(input);

            var insertedClient = await _clientRepository.InsertAsync(client);

            return ObjectMapper.Map<Client, ClientDto>(insertedClient);
        }

        public async Task<ClientDto> UpdateAsync(UpdateClientDto input)
        {
            var client = ObjectMapper.Map<UpdateClientDto, Client>(input);

            var updatedClient = await _clientRepository.UpdateAsync(client);

            return ObjectMapper.Map<Client, ClientDto>(updatedClient);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _clientRepository.DeleteAsync(id);
        }
    }
}
