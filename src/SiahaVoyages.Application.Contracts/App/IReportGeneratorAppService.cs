using System;
using Volo.Abp.Application.Services;

namespace SiahaVoyages.App
{
    public interface IReportGeneratorAppService : IApplicationService
    {
        byte[] GetByteDataVoucher(Guid voucherId);
        byte[] GetByteDataInvoice(Guid invoiceId);
    }
}
