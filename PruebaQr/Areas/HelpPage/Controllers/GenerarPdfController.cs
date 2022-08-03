using iText.Barcodes;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Borders;
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
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Image = iText.Layout.Element.Image;

namespace PruebaQr.Areas.HelpPage.Controllers
{
    [RoutePrefix("generarpdf")]
    public class GenerarPdfController : Controller
    {
        // GET: HelpPage/Text
        [AllowAnonymous]
        [Route("qrdemanda")]
        [HttpPost]
        public async Task<ActionResult> QrDemanda(QrDemandaDTo dto)
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

        [AllowAnonymous]
        [Route("fichatecnica")]
        [HttpPost]
        public async Task<ActionResult> FichaTecnica(FichaDto dto, bool isCantidad)
        {
            int index = 1;
            var hojastemp = dto.Items.Select(x => x.Cantid).Sum();
            int indexhojas = int.Parse(dto.Items.Select(x => x.Cantid).Sum().ToString());

            MemoryStream ms = new MemoryStream();
            PdfWriter pw = new PdfWriter(ms);
            PdfDocument pdfDocument = new PdfDocument(pw);
            float h = 500f;
            float w = 590f;
            PageSize pageSize = new PageSize(w, h);
            pdfDocument.SetDefaultPageSize(pageSize);


            Document doc = new Document(pdfDocument);

            for (int i = 0; i < dto.Items.Count; i++)
            {

                var datos = dto.Items[i];

                var qrContent = $"{datos.TipPro}{datos.ArtCod}".Replace(" ", "");

                BarcodeQRCode qrCode = new BarcodeQRCode(qrContent);
                PdfFormXObject barcodeObject = qrCode.CreateFormXObject(ColorConstants.BLACK, pdfDocument);
                Image barcodeImage = new Image(barcodeObject).SetWidth(110f).SetHeight(110f);
                if (isCantidad)
                {
                    for (int j = 0; j < datos.Cantid; j++)
                    {

                        ImageData imagedataEmpresa = ImageDataFactory.Create(dto.EmpImg);

                        Image imageempresa = new Image(imagedataEmpresa);
                        imageempresa.SetHeight(70f);
                        imageempresa.SetWidth(120f);

                        ImageData imagedataFicha = ImageDataFactory.Create(dto.EmpImg); // revisar
                        imagedataFicha.SetWidth(110f);
                        imagedataFicha.SetHeight(110f);

                        Image imagemedio = new Image(imagedataFicha);
                        imagemedio.SetWidth(110f);
                        imagemedio.SetHeight(110f);

                        Paragraph p0 = new Paragraph("");

                        //Paragraph p1 = new Paragraph("lorito");
                        //p1.SetFontSize(8);
                        //p1.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));

                        Paragraph p2 = new Paragraph("Ficha técnica de producto terminado");
                        p2.SetFontSize(20);
                        p2.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));

                        Paragraph p3 = new Paragraph("preparado por: ");
                        p3.SetFontSize(8);
                        p3.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));

                        Paragraph p4 = new Paragraph("aprobado por: ");
                        p4.SetFontSize(8);
                        p4.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));



                        Paragraph p5 = new Paragraph("fecha: ");
                        p5.SetFontSize(8);
                        p5.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));

                        Paragraph p6 = new Paragraph("versión: ");
                        p6.SetFontSize(8);
                        p6.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));

                        Paragraph p7 = new Paragraph(dto.EmpNom);
                        p7.SetFontSize(15);
                        p7.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                        p7.SetBold();


                        //Paragraph p8 = new Paragraph();
                        //p8.Add(new Chunk())


                        Paragraph p9 = new Paragraph("Imagen temporal");
                        p9.SetFontSize(8);
                        p9.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));

                        Paragraph p10 = new Paragraph("DESCRIPCION COMPLETA");
                        p10.SetFontSize(8);
                        p10.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                        p10.SetBold();

                        Paragraph p11 = new Paragraph("CÓDIGO ÚNICO");
                        p11.SetFontSize(8);
                        p11.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                        p11.SetBold();
                        Paragraph p12 = new Paragraph("TIPO DE FAMILIA");
                        p12.SetFontSize(8);
                        p12.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                        p12.SetBold();
                        Paragraph p13 = new Paragraph("CÓDIGO ERP");
                        p13.SetFontSize(8);
                        p13.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                        p13.SetBold();
                        Paragraph p14 = new Paragraph("UBICACIÓN");
                        p14.SetFontSize(8);
                        p14.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                        p14.SetBold();
                        Paragraph p15 = new Paragraph("PRESENTACIÓN");
                        p15.SetFontSize(8);
                        p15.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                        p15.SetBold();

                        Paragraph linebreak = new Paragraph("\n");
                        float[] pointColumnWidths = { 140F, 170F, 105F, 105f };
                        Table table = new Table(pointColumnWidths);
                        table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        table.AddCell(new Cell(1, 1).Add(imageempresa.SetHorizontalAlignment(HorizontalAlignment.CENTER)));
                        table.AddCell(new Cell(1, 3).Add(p2).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(new Cell(1, 1).Add(p3));
                        table.AddCell(new Cell(1, 1).Add(p4));
                        table.AddCell(new Cell(1, 1).Add(p5));
                        table.AddCell(new Cell(1, 1).Add(p6));

                        doc.Add(table);


                        ////
                        //float[] pointColumnWidths2 = { 140F, 170F, 105F, 105f };
                        //Table table2 = new Table(pointColumnWidths2);
                        //table2.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        //table2.AddCell(new Cell().Add(p3));
                        //table2.AddCell(new Cell().Add(p4));
                        //table2.AddCell(new Cell().Add(p5));
                        //table2.AddCell(new Cell().Add(p6));
                        //doc.Add(table2);

                        doc.Add(linebreak);

                        float[] pointColumnWidths4 = { 130F, 130F, 130F, 130f };

                        Table table3 = new Table(pointColumnWidths4);
                        table3.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        table3.AddCell(new Cell(1, 4).Add(p7)).SetTextAlignment(TextAlignment.CENTER);
                        table3.AddCell(new Cell(1, 2).Add(barcodeImage.SetHorizontalAlignment(HorizontalAlignment.CENTER)).SetBorderTop(new SolidBorder(ColorConstants.BLACK, 0.6f)).SetBorderBottom(new SolidBorder(ColorConstants.BLACK, 0.6f)));
                        table3.AddCell(new Cell(1, 2).Add(imagemedio.SetHorizontalAlignment(HorizontalAlignment.CENTER)).SetBorderLeft(new SolidBorder(ColorConstants.WHITE, 0.6f)).SetBorderTop(new SolidBorder(ColorConstants.BLACK, 0.6f)).SetBorderBottom(new SolidBorder(ColorConstants.BLACK, 0.6f)));
                        doc.Add(table3);

                        //Cell imagecell = new Cell(1, 4);
                        //imagecell.Add(barcodeImage.SetFixedPosition(40f, 124f));
                        //imagecell.Add(barcodeImage.SetFixedPosition(40f, 124f));

                        //Table table4 = new Table(pointColumnWidths4);


                        //table4.SetHeight(120f);
                        ////table4.SetBorderBottom(new SolidBorder(ColorConstants.BLACK, 1));
                        ////table4.SetBorderTop(new SolidBorder(ColorConstants.BLACK, 1));
                        //doc.Add(table4);

                        float[] pointColumnWidths5 = { 140F, 170F };
                        Table table5 = new Table(pointColumnWidths5);
                        table5.SetHorizontalAlignment(HorizontalAlignment.LEFT);
                        table5.AddCell(new Cell().Add(p10));
                        table5.AddCell(new Cell().Add(new Paragraph(datos.DscPro)));
                        table5.AddCell(new Cell().Add(p11));
                        table5.AddCell(new Cell().Add(new Paragraph(datos.CodUnc == null ? qrContent : datos.CodUnc)));
                        table5.AddCell(new Cell().Add(p12));
                        table5.AddCell(new Cell().Add(new Paragraph(datos.TipPro + " " + datos.DescTPr)));
                        table5.AddCell(new Cell().Add(p13));
                        table5.AddCell(new Cell().Add(new Paragraph(datos.ArtCod)));
                        table5.AddCell(new Cell().Add(p14));
                        table5.AddCell(new Cell().Add(new Paragraph(datos.UbiArt)));
                        table5.AddCell(new Cell().Add(p15));
                        table5.AddCell(new Cell().Add(new Paragraph(datos.UniMed)));
                        doc.Add(table5);
                    }
                }
                else
                {
                    ImageData imagedataEmpresa = ImageDataFactory.Create(dto.EmpImg);

                    Image imageempresa = new Image(imagedataEmpresa);
                    imageempresa.SetHeight(70f);
                    imageempresa.SetWidth(120f);

                    ImageData imagedataFicha = ImageDataFactory.Create(dto.EmpImg); // revisar
                    imagedataFicha.SetWidth(110f);
                    imagedataFicha.SetHeight(110f);

                    Image imagemedio = new Image(imagedataFicha);
                    imagemedio.SetWidth(110f);
                    imagemedio.SetHeight(110f);

                    Paragraph p0 = new Paragraph("");

                    //Paragraph p1 = new Paragraph("lorito");
                    //p1.SetFontSize(8);
                    //p1.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));

                    Paragraph p2 = new Paragraph("Ficha técnica de producto terminado");
                    p2.SetFontSize(20);
                    p2.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));

                    Paragraph p3 = new Paragraph("preparado por: ");
                    p3.SetFontSize(8);
                    p3.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));

                    Paragraph p4 = new Paragraph("aprobado por: ");
                    p4.SetFontSize(8);
                    p4.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));



                    Paragraph p5 = new Paragraph("fecha: ");
                    p5.SetFontSize(8);
                    p5.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));

                    Paragraph p6 = new Paragraph("versión: ");
                    p6.SetFontSize(8);
                    p6.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));

                    Paragraph p7 = new Paragraph(dto.EmpNom);
                    p7.SetFontSize(15);
                    p7.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                    p7.SetBold();


                    //Paragraph p8 = new Paragraph();
                    //p8.Add(new Chunk())


                    Paragraph p9 = new Paragraph("Imagen temporal");
                    p9.SetFontSize(8);
                    p9.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));

                    Paragraph p10 = new Paragraph("DESCRIPCION COMPLETA");
                    p10.SetFontSize(8);
                    p10.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                    p10.SetBold();

                    Paragraph p11 = new Paragraph("CÓDIGO ÚNICO");
                    p11.SetFontSize(8);
                    p11.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                    p11.SetBold();
                    Paragraph p12 = new Paragraph("TIPO DE FAMILIA");
                    p12.SetFontSize(8);
                    p12.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                    p12.SetBold();
                    Paragraph p13 = new Paragraph("CÓDIGO ERP");
                    p13.SetFontSize(8);
                    p13.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                    p13.SetBold();
                    Paragraph p14 = new Paragraph("UBICACIÓN");
                    p14.SetFontSize(8);
                    p14.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                    p14.SetBold();
                    Paragraph p15 = new Paragraph("PRESENTACIÓN");
                    p15.SetFontSize(8);
                    p15.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                    p15.SetBold();

                    Paragraph linebreak = new Paragraph("\n");
                    float[] pointColumnWidths = { 140F, 170F, 105F, 105f };
                    Table table = new Table(pointColumnWidths);
                    table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                    table.AddCell(new Cell(1, 1).Add(imageempresa.SetHorizontalAlignment(HorizontalAlignment.CENTER)));
                    table.AddCell(new Cell(1, 3).Add(p2).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                    table.AddCell(new Cell(1, 1).Add(p3));
                    table.AddCell(new Cell(1, 1).Add(p4));
                    table.AddCell(new Cell(1, 1).Add(p5));
                    table.AddCell(new Cell(1, 1).Add(p6));

                    doc.Add(table);


                    ////
                    //float[] pointColumnWidths2 = { 140F, 170F, 105F, 105f };
                    //Table table2 = new Table(pointColumnWidths2);
                    //table2.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                    //table2.AddCell(new Cell().Add(p3));
                    //table2.AddCell(new Cell().Add(p4));
                    //table2.AddCell(new Cell().Add(p5));
                    //table2.AddCell(new Cell().Add(p6));
                    //doc.Add(table2);

                    doc.Add(linebreak);

                    float[] pointColumnWidths4 = { 130F, 130F, 130F, 130f };

                    Table table3 = new Table(pointColumnWidths4);
                    table3.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                    table3.AddCell(new Cell(1, 4).Add(p7)).SetTextAlignment(TextAlignment.CENTER);
                    table3.AddCell(new Cell(1, 2).Add(barcodeImage.SetHorizontalAlignment(HorizontalAlignment.CENTER)).SetBorderTop(new SolidBorder(ColorConstants.BLACK, 0.6f)).SetBorderBottom(new SolidBorder(ColorConstants.BLACK, 0.6f)));
                    table3.AddCell(new Cell(1, 2).Add(imagemedio.SetHorizontalAlignment(HorizontalAlignment.CENTER)).SetBorderLeft(new SolidBorder(ColorConstants.WHITE, 0.6f)).SetBorderTop(new SolidBorder(ColorConstants.BLACK, 0.6f)).SetBorderBottom(new SolidBorder(ColorConstants.BLACK, 0.6f)));
                    doc.Add(table3);

                    //Cell imagecell = new Cell(1, 4);
                    //imagecell.Add(barcodeImage.SetFixedPosition(40f, 124f));
                    //imagecell.Add(barcodeImage.SetFixedPosition(40f, 124f));

                    //Table table4 = new Table(pointColumnWidths4);
                    

                    //table4.SetHeight(120f);
                    ////table4.SetBorderBottom(new SolidBorder(ColorConstants.BLACK, 1));
                    ////table4.SetBorderTop(new SolidBorder(ColorConstants.BLACK, 1));
                    //doc.Add(table4);

                    float[] pointColumnWidths5 = { 140F, 170F };
                    Table table5 = new Table(pointColumnWidths5);
                    table5.SetHorizontalAlignment(HorizontalAlignment.LEFT);
                    table5.AddCell(new Cell().Add(p10));
                    table5.AddCell(new Cell().Add(new Paragraph(datos.DscPro)));
                    table5.AddCell(new Cell().Add(p11));
                    table5.AddCell(new Cell().Add(new Paragraph(datos.CodUnc == null ? qrContent : datos.CodUnc)));
                    table5.AddCell(new Cell().Add(p12));
                    table5.AddCell(new Cell().Add(new Paragraph(datos.TipPro + " " + datos.DescTPr)));
                    table5.AddCell(new Cell().Add(p13));
                    table5.AddCell(new Cell().Add(new Paragraph(datos.ArtCod)));
                    table5.AddCell(new Cell().Add(p14));
                    table5.AddCell(new Cell().Add(new Paragraph(datos.UbiArt)));
                    table5.AddCell(new Cell().Add(p15));
                    table5.AddCell(new Cell().Add(new Paragraph(datos.UniMed)));
                    doc.Add(table5);
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