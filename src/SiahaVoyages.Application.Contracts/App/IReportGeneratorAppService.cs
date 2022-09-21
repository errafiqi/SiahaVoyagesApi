using SiahaVoyages.App.Dtos;
using Volo.Abp.Application.Services;

namespace SiahaVoyages.App
{
    public interface IReportGeneratorAppService : IApplicationService
    {
        byte[] GetByteDataVoucher(VoucherDto voucher);
        byte[] GetByteDataInvoice(InvoiceDto invoice);
    }
}
