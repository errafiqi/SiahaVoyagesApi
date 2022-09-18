using DinkToPdf;
using DinkToPdf.Contracts;
using System;
using System.Text;
using Volo.Abp.Application.Services;

namespace SiahaVoyages.App
{
    public class ReportGeneratorAppService : ApplicationService, IReportGeneratorAppService
    {
        private readonly IConverter _converter;

        public ReportGeneratorAppService(IConverter converter)
        {
            _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public byte[] GetByteDataVoucher(Guid voucherId)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            };

            var objetSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = GetHtmlContentVoucher(voucherId),
                WebSettings = { DefaultEncoding = "utf-8" }
            };

            var pdf = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objetSettings }
            };
			var file = _converter.Convert(pdf);
            return file;
        }

        private string GetHtmlContentVoucher(Guid voucherId)
        {
            var sb = new StringBuilder();

            sb.Append(@"");

            return sb.ToString();
        }


        public byte[] GetByteDataInvoice(Guid invoiceId)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            };

            var objetSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = GetHtmlContentInvoice(invoiceId),
                WebSettings = { DefaultEncoding = "utf-8" }
            };

            var pdf = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objetSettings }
            };

            var file = _converter.Convert(pdf);
            return file;
        }

        private string GetHtmlContentInvoice(Guid invoiceId)
        {
            var sb = new StringBuilder();

            sb.Append(@"");

            return sb.ToString();
        }

    }
}
