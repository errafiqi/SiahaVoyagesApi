using SiahaVoyages.App.Dtos;
using SiahaVoyages.App.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace SiahaVoyages.App
{
    public class TransferAppService : SiahaVoyagesAppService, ITransferAppService
    {
        IRepository<Transfer, Guid> _transferRepository;

        IRepository<Client, Guid> _clientRepository;

        ICurrentUser _currentUser;

        public TransferAppService(IRepository<Transfer, Guid> TransferRepository, ICurrentUser currentUser,
            IRepository<Client, Guid> clientRepository)
        {
            _transferRepository = TransferRepository;
            _currentUser = currentUser;
            _clientRepository = clientRepository;
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
                .WhereIf(!string.IsNullOrEmpty(input.Filter), t => t.Client != null
                    && (t.Client.User.Name + " " + t.Client.User.Surname).Contains(input.Filter))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(d => d.LastModificationTime != null ? d.LastModificationTime : d.CreationTime)
                .ToList();

            var totalCount = transfers.Any() ? transfers.Count() : 0;

            return new PagedResultDto<TransferDto>(
                totalCount,
                ObjectMapper.Map<List<Transfer>, List<TransferDto>>(transfers)
            );
        }

        public async Task<PagedResultDto<TransferDto>> GetListHistoriqueAsync(Guid userId, GetTransferListDto input)
        {
            var query = await _transferRepository.WithDetailsAsync(t => t.Client, t => t.Client.User, t => t.Driver, t => t.Driver.User);

            var transfers = query.Where(t => t.Client.UserId == userId)
                .WhereIf(!string.IsNullOrEmpty(input.Filter), t => t.Client != null
                    && (t.Client.User.Name + " " + t.Client.User.Surname).Contains(input.Filter))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(d => d.LastModificationTime != null ? d.LastModificationTime : d.CreationTime)
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

        public async Task<TransferDto> UpdateAsync(Guid TransferId, UpdateTransferDto input)
        {
            var transfer = await _transferRepository.GetAsync(TransferId);
            transfer.ClientId = input.ClientId;
            transfer.Passengers = input.PassengersNamesString;
            transfer.PassengersPhone = input.PassengersPhone;
            transfer.FMNO = input.FMNO;
            transfer.PickupDate = input.PickupDate;
            transfer.PickupPoint = input.PickupPoint;
            transfer.DeliveryDate = input.DeliveryDate;
            transfer.DeliveryPoint = input.DeliveryPoint;
            transfer.DriverReview = input.DriverReview;
            transfer.Rate = input.Rate;
            transfer.From = input.From;
            transfer.To = input.To;
            transfer.IsAirportTransfert = input.IsAirportTransfert;
            transfer.FlightDetails = input.FlightDetails;
            transfer.Price = input.Price;
            transfer.DriverId = input.DriverId;

            var updatedTransfer = await _transferRepository.UpdateAsync(transfer);

            return ObjectMapper.Map<Transfer, TransferDto>(updatedTransfer);
        }

        public async Task<TransferDto> DeleteAsync(Guid id)
        {
            var transfer = await _transferRepository.GetAsync(id);

            await _transferRepository.DeleteAsync(id);

            return ObjectMapper.Map<Transfer, TransferDto>(transfer);
        }

        public async Task<TransferDto> CancelAsync(Guid id)
        {
            var transfer = (await _transferRepository.WithDetailsAsync(t => t.Driver))
                    .FirstOrDefault(t => t.Id == id);
            transfer.State = TransferStateEnum.Canceled;

            if (transfer.DriverId != null)
            {
                var count = await _transferRepository.CountAsync(t => t.Id != id && t.DriverId == transfer.DriverId);
                if (count == 0) transfer.Driver.Available = true;
            }

            var updatedTransfer = await _transferRepository.UpdateAsync(transfer);

            return ObjectMapper.Map<Transfer, TransferDto>(updatedTransfer);
        }
    }
}
