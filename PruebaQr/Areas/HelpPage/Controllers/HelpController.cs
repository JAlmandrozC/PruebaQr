using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using PruebaQr.Areas.HelpPage.ModelDescriptions;
using PruebaQr.Areas.HelpPage.Models;
using PruebaQr.LL;
using PruebaQr.Models;
using QRCoder;
using Spire.Pdf;
using Spire.Pdf.Graphics;

namespace PruebaQr.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    /// 
    [RoutePrefix("pruebas")]
    public class HelpController : Controller
    {
        protected IQrService _service;

        public HelpController()
        {
            _service = new QrService();
        }


        [Route("qr")]
        [HttpGet]
        public async Task<ActionResult> QrExport(QrDto dto)
        {
            //Metodo para cargar las imagenes

            var imageLogo1 = _service.GetImage(dto.ImageUrl1);
            var imageLogo2 = _service.GetImage(dto.ImageUrl2);

            //Con esto se crea el tipo de documento (pdf)
            PdfDocument doc = new PdfDocument();

            //Agregar paginas que tendra el pdf ( en este caso solo hay 1 )
            PdfPageBase page = doc.Pages.Add(PdfPageSize.A6, new PdfMargins(0));

   
            var qrCode = _service.CreateQr(dto.Id);
            PdfImage image;
            MemoryStream img = new MemoryStream();

            using (Bitmap bitMap = qrCode.GetGraphic(10))
            {
                Bitmap resized = new Bitmap(bitMap, new Size(290, 290));

                resized.Save(img, ImageFormat.Png);

                image = PdfImage.FromStream(img);


            }






            using (MemoryStream ms = new MemoryStream())
            {


                // ----FreeSpirePDF - Libreria de Nugget



                PdfGraphicsState state = page.Canvas.Save();

                //si se incluye texto, con esto se modifica los parametros que tendra ( estilos )
                PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("Helvetica", 20f, FontStyle.Bold), true);


                //dibujar figuras, en este caso un recantigulo que cubrira al pdf de azul para simular un fondo
            
               var brush = _service.SetColor(dto.RedBody,dto.GreenBody,dto.BlueBody);

                
                page.Canvas.DrawRectangle(brush, new RectangleF(new Point(0, 0), PdfPageSize.A6));
                page.Canvas.Restore(state);


                //parametro y metodo para cargar las imagenes
                PdfImage logo1 = PdfImage.FromStream(_service.ToStream(imageLogo1));
                
                PdfImage logo2 = PdfImage.FromStream(_service.ToStream(imageLogo2));
                
                


                //transformar el QR a imagen
                

                //una variable temporal que asimila el valor mas cercano para centrar el gráfico de QR
                var tempwidth = float.Parse((79.4).ToString()) / 2;

                //dibujar las imagenes
                var brushRect = _service.SetColor(255, 255, 255);

                page.Canvas.DrawRectangle(brushRect, new Rectangle(new Point(7, 7), new Size(125, 50)));
                page.Canvas.DrawRectangle(brushRect, new Rectangle(new Point(165, 7), new Size(125, 50)));
                page.Canvas.DrawImage(logo1, new Rectangle(new Point(10, 10), new Size(120, 45)));
                page.Canvas.DrawImage(logo2, new PointF(168, 10), new SizeF(120, 45));

                //footer

                var brushFooter = _service.SetColor(dto.RedFooter, dto.GreenFooter, dto.BlueFooter);


                page.Canvas.DrawRectangle(brushFooter, new RectangleF(new Point(0, 350), PdfPageSize.A6));
                page.Canvas.Restore(state);

                //Texto

                PdfStringFormat leftAlignment = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                PdfStringFormat rightAlignment = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);

                page.Canvas.DrawString(dto.Label1, font, new PdfSolidBrush(Color.Black), 0, 370,leftAlignment);

                page.Canvas.DrawString(dto.TextFooter1, font, new PdfSolidBrush(Color.Black), 0, 400, leftAlignment);

                page.Canvas.DrawString(dto.Label2, font, new PdfSolidBrush(Color.Black), page.Canvas.ClientSize.Width, 370, rightAlignment);

                page.Canvas.DrawString(dto.TextFooter2, font, new PdfSolidBrush(Color.Black), page.Canvas.ClientSize.Width, 400, rightAlignment);
                //page.Canvas.DrawString(dto.TextFooter1, font, new PdfSolidBrush(Color.Black), page.Canvas.ClientSize.Width / 2, 400, centerAlignment);



                page.Canvas.DrawImage(image, new PointF(tempwidth, 100));
                doc.SaveToStream(ms);
                doc.Close();
                return File(ms.ToArray(), "application/pdf", "qr_exportado.pdf");

            }
        }

        [Route("qr/custom")]
        [HttpGet]
        public async Task<ActionResult> QrCustomExport(int idempresa, string idproceso)
        {
            //Metodo para cargar las imagenes


            //Con esto se crea el tipo de documento (pdf)
            PdfDocument doc = new PdfDocument();

            //Agregar paginas que tendra el pdf ( en este caso solo hay 1 )
            PdfPageBase page = doc.Pages.Add(new SizeF(400, 150), new PdfMargins(0));

            using (MemoryStream ms = new MemoryStream())
            {

                // ----QRCoder - Libreria de Nugget
                var qrCode = _service.CreateCustomQr(idproceso);

                // ----FreeSpirePDF - Libreria de Nugget



                PdfGraphicsState state = page.Canvas.Save();

                //si se incluye texto, con esto se modifica los parametros que tendra ( estilos )
                PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("Helvetica", 20f, FontStyle.Bold), true);

                PdfPen pen1 = new PdfPen(Color.Black, 1f);
                page.Canvas.DrawRectangle(pen1, new Rectangle(new Point(5, 5), new Size(390, 140)));

                //dibujar figuras, en este caso un recantigulo que cubrira al pdf de azul para simular un fondo




                //parametro y metodo para cargar las imagenes





                //transformar el QR a imagen


                //una variable temporal que asimila el valor mas cercano para centrar el gráfico de QR
                var tempwidth = float.Parse((79.4).ToString()) / 2;

                //dibujar las imagenes
                //PdfPen pen1 = new PdfPen(Color.Red, 1f);
                //page.Canvas.DrawRectangle(pen1, new Rectangle(new Point(7, 7), new Size(125, 50)));
                //page.Canvas.DrawRectangle(pen1, new Rectangle(new Point(162, 7), new Size(125, 50)));

                //footer

                //var brushFooter = _service.SetColor(dto.ColorFooter);


                //page.Canvas.DrawRectangle(brushFooter, new RectangleF(new Point(0, 350), PdfPageSize.A6));
                //page.Canvas.Restore(state);

                //Texto

                //PdfStringFormat centerAlignment = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);

                page.Canvas.DrawString("D001-S001", font, new PdfSolidBrush(Color.Black), 150, 10);

                page.Canvas.DrawString("EMPRESA", font, new PdfSolidBrush(Color.Black), 270, 10);

                page.Canvas.DrawString("R02 0101002N", font, new PdfSolidBrush(Color.Black), 150, 40);

                page.Canvas.DrawString("PERNOS-12", font, new PdfSolidBrush(Color.Black), 150, 70);

                page.Canvas.DrawString("UBICACIÓN", font, new PdfSolidBrush(Color.Black), 150, 100);

                //page.Canvas.DrawString(dto.TextFooter1 + "   " + dto.TextFooter2, font, new PdfSolidBrush(Color.Black), page.Canvas.ClientSize.Width / 2, 400, centerAlignment);
                MemoryStream img = new MemoryStream();
                PdfImage image;
                using (Bitmap bitMap = qrCode.GetGraphic(10))
                {
                    Bitmap resized = new Bitmap(bitMap, new Size(170, 170));

                    resized.Save(img, ImageFormat.Png);

                    image = PdfImage.FromStream(img);


                }

                page.Canvas.DrawImage(image, new PointF(10, 10));
                doc.SaveToStream(ms);
                doc.Close();
                return File(ms.ToArray(), "application/pdf", "qrcustom_exportado.pdf");

            }
        }

        //[Route("qr/lista")]
        //[HttpGet]
        //public async Task<ActionResult> QrListExport(List<QrDto> dto)
        //{
        //    //Metodo para cargar las imagenes
        //    PdfDocument doc = new PdfDocument();
        //    PdfPageBase page;


        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        foreach (var item in dto)
        //    { 
        //    var imageLogo1 = _service.GetImage(item.ImageUrl1);
        //    var imageLogo2 = _service.GetImage(item.ImageUrl2);



        //            // ----QRCoder - Libreria de Nugget
        //            var qrCode = _service.CreateQr(item.Id);

        //            // ----FreeSpirePDF - Libreria de Nugget

        //            //Con esto se crea el tipo de documento (pdf)


        //            //Agregar paginas que tendra el pdf ( en este caso solo hay 1 )

        //                page = doc.Pages.Add(PdfPageSize.A6, new PdfMargins(0));

        //            //si se incluye texto, con esto se modifica los parametros que tendra ( estilos )
        //            PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("Helvetica", 20f, FontStyle.Bold), true);


        //            //dibujar figuras, en este caso un recantigulo que cubrira al pdf de azul para simular un fondo

        //            var brush = _service.SetColor(item.ColorBody);


        //            page.Canvas.DrawRectangle(brush, new RectangleF(new Point(0, 0), PdfPageSize.A6));
             


        //            //parametro y metodo para cargar las imagenes
        //            PdfImage logo1 = PdfImage.FromStream(_service.ToStream(imageLogo1));

        //            PdfImage logo2 = PdfImage.FromStream(_service.ToStream(imageLogo2));




        //            //transformar el QR a imagen
        //            MemoryStream img = new MemoryStream();

        //            //una variable temporal que asimila el valor mas cercano para centrar el gráfico de QR
        //            var tempwidth = float.Parse((79.4).ToString()) / 2;

        //            //dibujar las imagenes
        //            PdfPen pen1 = new PdfPen(Color.Red, 1f);
        //            page.Canvas.DrawRectangle(pen1, new Rectangle(new Point(7, 7), new Size(125, 50)));
        //            page.Canvas.DrawRectangle(pen1, new Rectangle(new Point(165, 10), new Size(120, 50)));
        //            page.Canvas.DrawImage(logo1, new Rectangle(new Point(10, 10), new Size(120, 45)));
        //            page.Canvas.DrawImage(logo2, new PointF(165, 10), new SizeF(120, 50));

        //            //footer

        //            var brushFooter = _service.SetColor(item.ColorFooter);


        //            page.Canvas.DrawRectangle(brushFooter, new RectangleF(new Point(0, 350), PdfPageSize.A6));
      

        //            //Texto
        //            page.Canvas.DrawString(item.Label1, font, new PdfSolidBrush(Color.Black), 10, 350);
        //            page.Canvas.DrawString(item.Label2, font, new PdfSolidBrush(Color.Black), 100, 350);
        //            page.Canvas.DrawString(item.TextFooter1, font, new PdfSolidBrush(Color.Black), 10, 370);
        //            page.Canvas.DrawString(item.TextFooter2, font, new PdfSolidBrush(Color.Black), 100, 370);

                    //PdfImage image;
                    //using (Bitmap bitMap = qrCode.GetGraphic(10))
                    //{
                    //    Bitmap resized = new Bitmap(bitMap, new Size(290, 290));

                    //    resized.Save(img, ImageFormat.Png);

                    //    image = PdfImage.FromStream(img);

                    //}

        //            page.Canvas.DrawImage(image, new PointF(tempwidth, 100));
        //            doc.SaveToStream(ms);

                    
        //        }
        //        doc.Close();
        //        return File(ms.ToArray(), "application/pdf", "qr_exportado.pdf");

        //    }
        //}

        private string[] GetPath()
        {
            string path = Server.MapPath("~/Images/");
            string[] images = Directory.GetFiles(path);
            return images;
        }
    }
}