using SiahaVoyages.App.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace SiahaVoyages.App
{
    public class VoucherAppService : SiahaVoyagesAppService, IVoucherAppService
    {
        IRepository<Voucher, Guid> _voucherRepository;

        public VoucherAppService(IRepository<Voucher, Guid> VoucherRepository)
        {
            _voucherRepository = VoucherRepository;
        }

        public async Task<VoucherDto> GetAsync(Guid id)
        {
            var voucher = await _voucherRepository.GetAsync(id);
            return ObjectMapper.Map<Voucher, VoucherDto>(voucher);
        }

        public async Task<PagedResultDto<VoucherDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var query = (await _voucherRepository.WithDetailsAsync(v => v.Transfer));

            var vouchers = query.Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(d => d.LastModificationTime != null ? d.LastModificationTime : d.CreationTime)
                .ToList();

            var totalCount = vouchers.Any() ? vouchers.Count() : 0;

            return new PagedResultDto<VoucherDto>(
                totalCount,
                ObjectMapper.Map<List<Voucher>, List<VoucherDto>>(vouchers)
            );
        }

        public async Task<VoucherDto> CreateAsync(CreateVoucherDto input)
        {
            var voucher = (await _voucherRepository.GetQueryableAsync())
                .Where(v => v.TransferId == input.TransferId)
                .FirstOrDefault();
            if (voucher != null)
            {
                await _voucherRepository.DeleteAsync(voucher.Id);
            }
            voucher = ObjectMapper.Map<CreateVoucherDto, Voucher>(input);

            var insertedVoucher = await _voucherRepository.InsertAsync(voucher);

            return ObjectMapper.Map<Voucher, VoucherDto>(insertedVoucher);
        }

        public async Task<VoucherDto> UpdateAsync(UpdateVoucherDto input)
        {
            var voucher = ObjectMapper.Map<UpdateVoucherDto, Voucher>(input);

            var updatedVoucher = await _voucherRepository.UpdateAsync(voucher);

            return ObjectMapper.Map<Voucher, VoucherDto>(updatedVoucher);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _voucherRepository.DeleteAsync(id);
        }
    }
}
