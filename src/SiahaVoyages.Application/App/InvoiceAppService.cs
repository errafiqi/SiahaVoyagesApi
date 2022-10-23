using SiahaVoyages.App.Dtos;
using SiahaVoyages.App.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace SiahaVoyages.App
{
    public class InvoiceAppService : SiahaVoyagesAppService, IInvoiceAppService
    {
        IRepository<Invoice, Guid> _invoiceRepository;

        IRepository<Transfer, Guid> _transferRepository;

        IRepository<Client, Guid> _clientRepository;

        IReportGeneratorAppService _reportGeneratorAppService;

        public InvoiceAppService(IRepository<Invoice, Guid> InvoiceRepository, IRepository<Transfer, Guid> transferRepository
            , IRepository<Client, Guid> clientRepository, IReportGeneratorAppService reportGeneratorAppService)
        {
            _invoiceRepository = InvoiceRepository;
            _transferRepository = transferRepository;
            _clientRepository = clientRepository;
            _reportGeneratorAppService = reportGeneratorAppService;
        }

        public async Task<InvoiceDto> GetAsync(Guid id)
        {
            var invoice = (await _invoiceRepository.WithDetailsAsync(i => i.Client))
                .Where(v => v.Id == id)
                .FirstOrDefault();
            return ObjectMapper.Map<Invoice, InvoiceDto>(invoice);
        }

        public async Task<PagedResultDto<InvoiceDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var query = (await _invoiceRepository.WithDetailsAsync(i => i.Client));

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
            try
            {
                input.Mois = input.Mois.AddDays(1);
                var invoice = (await _invoiceRepository.GetQueryableAsync())
                    .Where(i => i.ClientId == input.ClientId && i.Mois.CompareTo(input.Mois) == 0)
                    .FirstOrDefault();
                if (invoice != null)
                {
                    await _invoiceRepository.DeleteAsync(invoice.Id);
                }
                invoice = ObjectMapper.Map<CreateInvoiceDto, Invoice>(input);

                var transfers = (await _transferRepository.GetQueryableAsync())
                    .Where(t => t.ClientId == input.ClientId && t.State == TransferStateEnum.Closed
                        && t.PickupDate.Month == input.Mois.Month
                        && t.PickupDate.Year == input.Mois.Year)
                    .OrderBy(t => t.PickupDate)
                    .ToList();
                var transfersListResultDto = new ListResultDto<TransferDto>(ObjectMapper.Map<List<Transfer>, List<TransferDto>>(transfers));
                float prix = 0;
                if (transfersListResultDto.Items != null && transfersListResultDto.Items.Any())
                {
                    foreach (var transfer in transfersListResultDto.Items)
                    {
                        prix += transfer.Rate;
                    }
                }
                var clientDto = ObjectMapper.Map<Client, ClientDto>(await _clientRepository.GetAsync(input.ClientId));

                var invoiceDto = new InvoiceDto
                {
                    Client = clientDto,
                    Date = input.Date,
                    Reference = input.Reference,
                    Mois = input.Mois,
                    Transfers = transfersListResultDto,
                    Prix = prix
                };

                invoice.Prix = prix;
                invoice.File = _reportGeneratorAppService.GetByteDataInvoice(invoiceDto);
                var insertedInvoice = await _invoiceRepository.InsertAsync(invoice);

                return ObjectMapper.Map<Invoice, InvoiceDto>(insertedInvoice);
            }
            catch (Exception ex)
            {
                return null;
            }
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
