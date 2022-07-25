using iText.Barcodes;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using PruebaQr.Models;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Image = iText.Layout.Element.Image;

namespace PruebaQr.Areas.HelpPage.Controllers
{
    [RoutePrefix("itext")]
    public class TextController : Controller
    {
        // GET: HelpPage/Text
        [AllowAnonymous]
        [Route("prueba")]
        [HttpPost]
        public async Task<ActionResult> ITextTest(QrDemandaDTo dto)
        {
            int index = 1;
            var hojastemp = dto.ItemsImprimirDTo.Select(x => x.Cantid).Sum();
            int indexhojas = int.Parse(dto.ItemsImprimirDTo.Select(x => x.Cantid).Sum().ToString());
            MemoryStream ms = new MemoryStream();
            PdfWriter pw = new PdfWriter(ms);
            PdfDocument pdfDocument = new PdfDocument(pw);
            Document doc = new Document(pdfDocument);

            float h;
            h = 70.866141732f;
            float w = 212.5984252f;
            PageSize pageSize = new PageSize(w, h);

            
            for (int i = 0; i < dto.ItemsImprimirDTo.Count; i++)
            {
            
                var datos = dto.ItemsImprimirDTo[i];
                
                var qrContent = $"{datos.TipPro}{datos.ArtCod}".Replace(" ", "");

                BarcodeQRCode qrCode = new BarcodeQRCode(qrContent);
                PdfFormXObject barcodeObject = qrCode.CreateFormXObject(ColorConstants.BLACK, pdfDocument);
                Image barcodeImage = new Image(barcodeObject).SetWidth(30f).SetHeight(30f);

                //QRCodeGenerator qrGenerator = new QRCodeGenerator();
                //QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.L);
                //QRCode qrCode = new QRCode(qrCodeData);

                //System.Drawing.Image image;
                //MemoryStream img = new MemoryStream();
                //using (Bitmap bitMap = qrCode.GetGraphic(10))
                //{
                //    Bitmap resized = new Bitmap(bitMap, new Size(350,350));

                //    resized.Save(img, ImageFormat.Png);

                //    image = System.Drawing.Image.FromStream(img);
                //}

                //ImageData imagedata = ImageDataFactory.Create(image, System.Drawing.Color.Transparent);

                //Image imagereal = new Image(imagedata);

                for (int j = 0; j < datos.Cantid; j++)
                {
                   

                    String t = $"{dto.ItemsImprimirDTo[i].Deposi}-{dto.ItemsImprimirDTo[i].Sector}";
                    Paragraph p1 = new Paragraph(t);
                    p1.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                    p1.SetFontSize(5);

                    String t2 = dto.RazonSocial;
                    Paragraph p2 = new Paragraph(t2);
                    p2.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                    p2.SetFontSize(5);
                    p2.SetBold();

                    String t3 = $"{datos.TipPro}{datos.ArtCod}".Replace(" ", "");
                    t3 = t3 + " - " + datos.UniMed;
                    Paragraph p3 = new Paragraph(t3);
                    p3.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                    p3.SetFontSize(5);

                    String t4 = datos.Descrp;
                    var temp = transform(t4, 34);
                    Paragraph p4 = new Paragraph(temp);
                    p4.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                    p4.SetFontSize(4);
                    p4.SetBold();

                    String t5 = datos.UbiArt;
                    Paragraph p5 = new Paragraph(t5);
                    p5.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                    p5.SetFontSize(5);




                    pdfDocument.SetDefaultPageSize(pageSize);
                    barcodeImage.ScaleToFit(w, h);
                    barcodeImage.SetFixedPosition(index, 0, -1);

                    //imagereal.ScaleToFit(w, h);
                    //imagereal.SetFixedPosition(index, 0, 0);


                    //p6.SetFixedPosition(100, 60,60);


                    //doc.Add(new Paragraph("Hello World"));

                    //doc.Add(imagereal);
                    //doc.Add(p6);
                    doc.Add(barcodeImage);
                    doc.ShowTextAligned(p1, 80, 50, TextAlignment.LEFT, VerticalAlignment.TOP);
                    doc.ShowTextAligned(p2, w - 5, 60, TextAlignment.RIGHT, VerticalAlignment.TOP);
                    doc.ShowTextAligned(p3, 80, 40, TextAlignment.LEFT, VerticalAlignment.TOP);
                    doc.ShowTextAligned(p4, 80, 30, TextAlignment.JUSTIFIED, VerticalAlignment.TOP);
                    doc.ShowTextAligned(p5, 80, 15, TextAlignment.JUSTIFIED, VerticalAlignment.TOP);
                    //doc.ShowTextAligned(p2, 120, 60, TextAlignment.CENTER, VerticalAlignment.TOP);
                    //doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));


                   
                        
                    index++;
                    //if(index == indexhojas -1)
                    //pdfDocument.AddNewPage(); 
                }
                
                    
            }

            doc.Close();

            byte[] byteStream = ms.ToArray();

            ms = new MemoryStream();
            ms.Write(byteStream, 0, byteStream.Length);
            ms.Position = 0;

            return new FileStreamResult(ms, "application/pdf");
        }

        public string transform(string text,int limite)
        {
            int Length = text.Length;
            StringBuilder Append = new StringBuilder();
            for (int i = 0; i < Length; i += limite)
            {
                if (i + limite > Length) limite = Length - i;
                var Final = text.Substring(i, limite) + "\n";
                Append.Append(Final);

            }

            return Append.ToString();
        }
    }
}