using Microsoft.AspNetCore.Mvc;
using SiahaVoyages.App;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace SiahaVoyages.Controllers;

public class HomeController : AbpController
{
    IVoucherAppService _voucherAppService;

    IInvoiceAppService _invoiceAppService;

    public HomeController(IVoucherAppService voucherAppService, IInvoiceAppService invoiceAppService)
    {
        _voucherAppService = voucherAppService;
        _invoiceAppService = invoiceAppService;
    }

    public ActionResult Index()
    {
        return Redirect("~/swagger");
    }

    public async Task<ActionResult> GetVoucher(Guid Id)
    {
        var voucher = await _voucherAppService.GetAsync(Id);
        return File(voucher.File, "application/pdf", "Bon_" + voucher.Reference + ".pdf");
    }

    public async Task<ActionResult> GetInvoice(Guid Id)
    {
        var invoice = await _invoiceAppService.GetAsync(Id);
        return File(invoice.File, "application/pdf", "Facture_" + invoice.Reference + ".pdf");
    }
}
