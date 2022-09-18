using SiahaVoyages.App.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SiahaVoyages.App
{
    public interface IInvoiceAppService : IApplicationService
    {
        Task<InvoiceDto> GetAsync(Guid id);

        Task<PagedResultDto<InvoiceDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<InvoiceDto> CreateAsync(CreateInvoiceDto input);

        Task<InvoiceDto> UpdateAsync(UpdateInvoiceDto input);

        Task DeleteAsync(Guid id);
    }
}
