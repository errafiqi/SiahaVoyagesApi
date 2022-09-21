using DinkToPdf;
using DinkToPdf.Contracts;
using SiahaVoyages.App.Dtos;
using System;
using System.IO;
using System.Text;

namespace SiahaVoyages.App
{
    public class ReportGeneratorAppService : SiahaVoyagesAppService, IReportGeneratorAppService
    {
        private readonly IConverter _converter;

        public ReportGeneratorAppService(IConverter converter)
        {
            _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public byte[] GetByteDataVoucher(VoucherDto voucher)
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
                HtmlContent = GetHtmlContentVoucher(voucher),
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

        private string GetHtmlContentVoucher(VoucherDto voucher)
        {
            var sb = new StringBuilder();

            sb.Append(@"<!doctype html>
<html lang='fr'>
<head>
  <meta charset='utf-8'>
  <title>Bon de mission</title>
<style type='text/css'>
        @page {
          size: A5 portrait !important;
      }
      body{
         size: A5 portrait !important; 
      }
      @media print, screen {
            html, body { margin: 0px;padding: 0px;font-family: 'Arial', sans-serif;} 
            
            table thead tr th, table tr td, p, h1,h2,h3,h4,h5,h6, ul, li, span, table, div {font-family: 'Arial', sans-serif;}


          .headerBon {position: relative;width: 97%;min-height: 145px;margin-left:auto; margin-right:auto;}
              .headerBon div {top: 0px;padding: 12.65px; position:absolute;}
              .headerBon .divImg {left: 0px;width:40%;}
                  .headerBon .divImg img {height:100px;width:auto;max-width:300px;}

              .headerBon .divInfosBon {max-width:60%;right:0px;}
                  .headerBon .divInfosBon p {text-align: right !important;margin: 2px;font-weight: 400;font-size: 16px;
                  }
                  .headerBon .divInfosBon .pDate {font-size: 16px;font-weight:400;}
                  .headerBon .divInfosBon .pDateEmission {margin-top:10px;}  
          .bodyBon{
            width:100%;
          }
          .bodyBon .bonDetails {
            border:solid 1px black;
            width: 97%;
            margin-left: auto;
            margin-right: auto;
            padding:15px;
            font-size:12px !important;
          }
          
          td{
            padding-top : 5px !important;
            padding-bottom : 5px !important;
          }
          
          .content{
          font-weight:700;
          color:#1e4d6c !important;
          }

        .date{
          width:65% !important;
          }
          .hour{
          width:35% !important;
          }

          .notice{
          width:97%;
          margin-left: auto;
          margin-right: auto;
          }
          .warning{
          width:100%;
          margin-left: auto;
          margin-right: auto;
          }
          
          .notice p{
          text-align: center;
          font-style: italic;
          font-weight:600;
          font-size:12px;
          }
          .warning{
          padding-top:15px;
          padding-bottom:15px;
          background-color:#1e4d6c !important;
          -webkit-print-color-adjust: exact !important;
          }
          .warning p{
          text-align: center;
          font-style: italic;
          font-weight:600;
          font-size:12px;
          color: #fff !important;
          }
        
        .footerBon {
          position: fixed;
          bottom:15px;
          left:15px;
          right:15px;
        }
        .footerBon p {
          text-align:center;
          font-size:12px;
          line-height:6px !important;
        }
      }
      
</style>
</head>
<body>
  <div class='headerBon'>
	  <div class='divImg'>
	  	<img src='");
			sb.Append(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\assets", "logo-siaha-with-slogan-250.png"));
			sb.Append(@"'>
    </div>
	  <div class='divInfosBon'>
	  	<p>Bon N° <span class='content'> @[REF-BON]</span></p>
	  	<p class='pDate pDateEmission'>Date d’émission&nbsp; : <span class='content'>@[DATE-BON]</span> </p>
	  </div>
  </div>
  <div class='bodyBon'>
    <table class='bonDetails'>
      <tr>
        <td class='full-width'>Nom Chauffeur : <span class='content'>@[DRIVER-FULLNAME]</span></td>
      </tr>
      <tr>
        <td class='full-width'>Nom Passager : <span class='content'>@[PASSENGER-FULLNAME]</span></td>
      </tr>
      <tr>
        <td class='full-width'>Charge Code : <span class='content'>@[CHARGE-CODE]</span></td>
      </tr>
      <tr>
        <td class='full-width'>FMNO Passager : <span class='content'>@[FMNO-PASSENGER]</span></td>
      </tr>
      <tr>
        <td class='full-width'>Itinéraire : De <span class='content'>@[POINT-FROM]</span> À <span class='content'>@[POINT-TO]</span></td>
      </tr>
      <tr class='double-row'>
        <td class='date'>Lieu de départ : <span class='content'>@[POINT-FROM]</span></td>
        <td class='hour'>Heure :  <span class='content'>@[HOUR-FROM]</span></td>
      </tr>
      <tr class='double-row'>
        <td class='date'>Lieu d'arrivée : <span class='content'>@[POINT-TO]</span></td>
        <td class='hour'>Heure :  <span class='content'>@[HOUR-TO]</span></td>
      </tr>
      <tr>
        <td class='full-width'>Prix :  <span class='content'>@[PRICE]</span></td>
      </tr>
      <tr style='padding-top:10px;'>
        <td class='date'></td>
        <td class='full-width' style='text-align: center;padding-top:25px !important;padding-bottom:60px !important;font-weight:700;'>Signature</td>
      </tr>
    </table>
    <div class='notice'>
      <p>PS : Si cette course est personnelle, merci de la régler directement auprès du chaufeur vous n'avez pas besoin dans ce cas de remplir de bon</p>
    </div>
    <div class='warning'>
      <p>ATTENTION: Il ne faut compléter qu'un seul bon si vous avez votre chauffeur à disposition en indiquant les détails de votre trajet (Aller/retour). Merci</p>
    </div>
  </div>
  <div class='footerBon'>
    <p>SIAHA Voyages - www.siahavoyage.com - Email : travel@siahavoyage.com </p>
    <p>164,Bd. ambassadeur Ben Aicha App.N°16, Roches noires- Casablanca, Maroc</p>
  </div>
</body>
</html>");

            return sb.ToString();
        }


        public byte[] GetByteDataInvoice(InvoiceDto invoice)
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
                HtmlContent = GetHtmlContentInvoice(invoice),
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

        private string GetHtmlContentInvoice(InvoiceDto invoice)
        {
            var sb = new StringBuilder();

            sb.Append(@"
<!doctype html>
<html lang='fr'>
<head>
  <meta charset='utf-8'>
  <title>SIAHA Voyages - Facture</title>
<style type='text/css'>
        @page {
          size: A4 portrait !important;
      }
      body{
         size: A4 portrait !important; 
         position:relative;
         height:100%;
      }
      @media print, screen {
            html, body { margin: 0px;padding: 0px;font-family: 'Arial', sans-serif;} 
            
            table thead tr th, table tr td, p, h1,h2,h3,h4,h5,h6, ul, li, span, table, div {font-family: 'Arial', sans-serif;}


          .headerBon {position: relative;width: 97%;min-height: 145px;margin-left:auto; margin-right:auto;}
              .headerBon div {top: 0px;padding: 12.65px; position:absolute;}
              .headerBon .divImg {left: 0px;width:40%;}
                  .headerBon .divImg img {height:100px;width:auto;max-width:300px;}

              .headerBon .divInfosBon {max-width:60%;right:0px;}
                  .headerBon .divInfosBon p {text-align: right !important;margin: 2px;font-weight: 400;font-size: 16px;
                  }
                  .headerBon .divInfosBon .pDate {font-size: 16px;font-weight:400;}
                  .headerBon .divInfosBon .pDateEmission {margin-top:10px;}  
          .bodyBon{
            width:100%;
          }
          .bodyBon .bonDetails {
          
            width: 97%;
            margin-left: auto;
            margin-right: auto;
            border-collapse: collapse;
            font-size:12px !important;
            min-height:300px !important;
            min-height:550px !important;
            overflow: hidden;
          }
          
          td{
            padding : 10px;
          }
          
          .content{
          font-weight:700 !important;
          color:#1e4d6c !important;
          }

        .date{
          width:65% !important;
          }
          .hour{
          width:35% !important;
          }

          .notice{
          width:97%;
          margin-left: auto;
          margin-right: auto;
          }
          .warning{
          width:100%;
          margin-left: auto;
          margin-right: auto;
          }
          
          .notice p{
          text-align: center;
          font-style: italic;
          font-weight:600;
          font-size:12px;
          }
          .warning{
          padding-top:15px;
          padding-bottom:15px;
          background-color:#1e4d6c !important;
          -webkit-print-color-adjust: exact !important;
          }
          .warning p{
          text-align: center;
          font-style: italic;
          font-weight:600;
          font-size:12px;
          color: #fff !important;
          }
        
        .footerBon {
          position: fixed;
          bottom:0px;
          left:0px;
          right:0px;
        }
        .footerBon p {
          text-align:center;
          font-size:12px;
          line-height:6px !important;
        }

        .cordonneesFacture {
				position: relative;
				width: 100%;
				height: 200px;
			  }
      .spacer{height:20px;}

				.cordonneesFacture div {
					position: absolute;
					top: 0px;
					padding: 10px;
				}

				.cordonneesFacture .divPour {
					left: 0px;
					width: 50%;
				}

				.cordonneesFacture .divPour .divPourLabel {
						left: 0px;
					
				}
				.pPourDe {font-size: 14px;line-height:14px !important;}

				.cordonneesFacture .divPour .divPourContent {
						left: 20px;
				}
				.cordonneesFacture .divPour .divPourContent p, 
				.cordonneesFacture .divDe .divDeContent p  {font-size:14px;}
				
				.raison-sociale{line-height:40px !important; font-size:16px !important;}
				
				.adresse, .ville, .pays, .numTel, .ice{font-weight: 300 !important;letter-spacing:1px;}
				.adresse, .ville, .pays, .ice {text-transform:lowercase;}
				.adresse::first-letter, .ville::first-letter, .pays::first-letter, .ice::first-letter{text-transform:capitalize;}
				
				.cordonneesFacture .divDe {
					right: 0px;
					width: 40%;
				}
				.cordonneesFacture .divDe .divDeLabel {
						left: 0px;	
				}
				.cordonneesFacture .divDe .divDeContent {
						left: 20px;	
				}

					.cordonneesFacture div p {
						margin: 2px;
					}
					.cordonneesFacture div .pPourDe {
						color: #898989;
					}
					.cordonneesFacture div .numTel {
						margin-top: 20px;
					}
			.spacer{height:10px;}


        th:first-of-type{
          text-align: left !important;
          width:70%;
        }
        th:last-of-type{
          text-align: left !important;
          width:30%;

        }
        tbody td:first-of-type{
          text-align: left !important;
          width:70%;
        }
        tbody td:last-of-type{
          text-align: left !important;
          width:30%;

        }
        tfoot td:first-of-type{
          text-align: left !important;
          width:70%;
        }
        tfoot td:last-of-type{
          text-align: left !important;
          width:30%;

        }

        tbody tr td:first-of-type{
          border-right: solid 1px black !important;
        }
        tbody tr td:first-of-type{
          border-right: solid 1px black !important;
        }
        thead tr th{
          border: solid black 1px !important;
          padding : 10px;
        }
        tbody tr td{
          border: solid black 1px !important;
        }
        tfoot tr td{
          
          padding : 10px;
        }

      }


      
</style>
</head>
<body>
  <div class='headerBon'>
	  <div class='divImg'>
        <img src='");
			sb.Append(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\assets", "logo-siaha-with-slogan-250.png"));
			sb.Append(@"'>
	  </div>
	  <div class='divInfosBon'>
	  	<p>Réf. Facture <span class='content'> @[REF-FACTURE]</span></p>
	  	<p class='pDate pDateEmission'>Date d’émission&nbsp; : <span class='content'>@[DATE-BON]</span> </p>
	  </div>
  </div>
  <div class='cordonneesFacture'>
    <div class='divPour'>
      <div class='divPourContent'>
        <p class='pPourDe'>De :</p>
        <p class='raison-sociale'> SIAHA Voyages</p>
        <p class='adresse'>164,Bd.ambassadeur Ben Aicha App.N°16 - Roches noires</p>
        <p class='ville'>Casablanca - Maroc </p>
        <p class='numTel'>travel@siahavoyage.com </p>
        <p class='ice'>000080202000071</p>
      </div>
    </div>

    <div class='divDe'>
      <div class='divDeContent'>
        <p class='pPourDe'>À :</p>
        <p class='raison-sociale'>@[CLIENT-RAISON-SOCIALE]</p>
        <p class='adresse'>@[CLIENT-ADRESSE]</p>
        <p class='ville'>@[CLIENT-CITY]</p>
        <p class='pays'>@CLIENT-COUNTRY]</p>
        <p class='ice'>@CLIENT-ICE]</p>
      </div>
    </div>

  </div>
  <div class='bodyBon'>
    <table class='bonDetails'>
      <thead>
        <tr>
          <th>
            Désignation
          </th>
          <th>
            Montant
          </th>
        </tr>
      </thead>
      <tbody>
        
        <tr>
          <td>
            <p>Période : <span class='content'>@[MIN-PICKUP]</span>au <span class='content'>@[MAX-PICKUP]</span></p>
            <p>Nombre de missions : <span class='content'>@[NBR-MISSIONS] </span></p>
          </td>
          <td><span class='content'>@[TOTAL-MOIS]</span></td>
        </tr>
      
      </tbody>
      <tfoot>
        <tr>
          <td style='text-align:right !important;'><span class='content'>Total HT</span></td>
          <td>@[TOTAL-MOIS]</td>
        </tr>
        <tr>
          <td style='text-align:right !important;'><span class='content'>Montant TVA (14%)</span></td>
          <td>@[TOTAL-TVA]</td>
        </tr>
        <tr>
          <td style='text-align:right !important;'><span class='content'>Total TTC</span></td>
          <td>@[TOTAL-TTC]</td>
        </tr>
      </tfoot>
    </table>
    <div class='notice'>
      <p>Arrêter la Facture à la somme de : @[SOMME-LETTER] MAD</p>
    </div>
    <div class='warning footerBon'>
      <p>SIAHA Voyages - www.siahavoyage.com - Email : travel@siahavoyage.com - Capital: 100000DH </p>
      <p>164,Bd. ambassadeur Ben Aicha App.N°16, Roches noires- Casablanca, Maroc</p>
      <p>IF: 01106159 - RC: 65709 - CNSS: 7508049 - ICE : 000080202000071</p>
    </div>
  </div>

</body>
</html>
");

            return sb.ToString();
        }

    }
}
