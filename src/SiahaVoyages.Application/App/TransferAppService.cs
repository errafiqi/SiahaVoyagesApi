using SiahaVoyages.App.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace SiahaVoyages.App
{
    public class TransferAppService : SiahaVoyagesAppService, ITransferAppService
    {
        IRepository<Transfer, Guid> _transferRepository;

        public TransferAppService(IRepository<Transfer, Guid> TransferRepository)
        {
            _transferRepository = TransferRepository;
        }

        public async Task<TransferDto> GetAsync(Guid id)
        {
            var transfer = (await _transferRepository.WithDetailsAsync(t => t.Client, t => t.Client.User, 
                    t => t.Driver, t => t.Driver.User))
                    .FirstOrDefault(t => t.Id == id);
            return ObjectMapper.Map<Transfer, TransferDto>(transfer);
        }

        public async Task<PagedResultDto<TransferDto>> GetListAsync(GetTransferListDto input)
        {
            var query = await _transferRepository.WithDetailsAsync(t => t.Client, t => t.Client.User, t => t.Driver, t => t.Driver.User);

            var transfers = query
                .WhereIf(!string.IsNullOrEmpty(input.Filter), t => t.Client != null && t.Client.User.Name.Contains(input.Filter))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderBy(d => d.LastModificationTime != null ? d.LastModificationTime : d.CreationTime)
                .ToList();

            var totalCount = transfers.Any() ? transfers.Count() : 0;

            return new PagedResultDto<TransferDto>(
                totalCount,
                ObjectMapper.Map<List<Transfer>, List<TransferDto>>(transfers)
            );
        }

        public async Task<TransferDto> CreateAsync(CreateTransferDto input)
        {
            var transfer = ObjectMapper.Map<CreateTransferDto, Transfer>(input);

            var insertedTransfer = await _transferRepository.InsertAsync(transfer);

            return ObjectMapper.Map<Transfer, TransferDto>(insertedTransfer);
        }

        public async Task<TransferDto> UpdateAsync(UpdateTransferDto input)
        {
            var transfer = ObjectMapper.Map<UpdateTransferDto, Transfer>(input);

            var updatedTransfer = await _transferRepository.UpdateAsync(transfer);

            return ObjectMapper.Map<Transfer, TransferDto>(updatedTransfer);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _transferRepository.DeleteAsync(id);
        }
    }
}
