using SiahaVoyages.App.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace SiahaVoyages.App
{
    public class InvoiceAppService : SiahaVoyagesAppService, IInvoiceAppService
    {
        IRepository<Invoice, Guid> _invoiceRepository;

        public InvoiceAppService(IRepository<Invoice, Guid> InvoiceRepository)
        {
            _invoiceRepository = InvoiceRepository;
        }

        public async Task<InvoiceDto> GetAsync(Guid id)
        {
            var invoice = await _invoiceRepository.GetAsync(id);
            return ObjectMapper.Map<Invoice, InvoiceDto>(invoice);
        }

        public async Task<PagedResultDto<InvoiceDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var query = await _invoiceRepository.GetQueryableAsync();

            var invoices = query.Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(d => d.LastModificationTime != null ? d.LastModificationTime : d.CreationTime)
                .ToList();

            var totalCount = invoices.Any() ? invoices.Count() : 0;

            return new PagedResultDto<InvoiceDto>(
                totalCount,
                ObjectMapper.Map<List<Invoice>, List<InvoiceDto>>(invoices)
            );
        }

        public async Task<InvoiceDto> CreateAsync(CreateInvoiceDto input)
        {
            var invoice = ObjectMapper.Map<CreateInvoiceDto, Invoice>(input);

            var insertedInvoice = await _invoiceRepository.InsertAsync(invoice);

            return ObjectMapper.Map<Invoice, InvoiceDto>(insertedInvoice);
        }

        public async Task<InvoiceDto> UpdateAsync(UpdateInvoiceDto input)
        {
            var invoice = ObjectMapper.Map<UpdateInvoiceDto, Invoice>(input);

            var updatedInvoice = await _invoiceRepository.UpdateAsync(invoice);

            return ObjectMapper.Map<Invoice, InvoiceDto>(updatedInvoice);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _invoiceRepository.DeleteAsync(id);
        }
    }
}
