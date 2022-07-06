using System;
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
            string[] images = GetPath();


            
            using (MemoryStream ms = new MemoryStream())
            {

                // ----QRCoder - Libreria de Nugget
                var qrCode = _service.CreateQr(dto.Id);

                // ----FreeSpirePDF - Libreria de Nugget

                //Con esto se crea el tipo de documento (pdf)
                PdfDocument doc = new PdfDocument();

                //Agregar paginas que tendra el pdf ( en este caso solo hay 1 )
                PdfPageBase page = doc.Pages.Add(PdfPageSize.A6, new PdfMargins(0));

                PdfGraphicsState state = page.Canvas.Save();

                //si se incluye texto, con esto se modifica los parametros que tendra ( estilos )
                PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("Helvetica", 12f, FontStyle.Bold), true);


                //dibujar figuras, en este caso un recantigulo que cubrira al pdf de azul para simular un fondo
                PdfBrush brush = new PdfSolidBrush(Color.Blue);
                
                page.Canvas.DrawRectangle(brush, new RectangleF(new Point(0, 0), PdfPageSize.A6));
                page.Canvas.Restore(state);


                //parametro y metodo para cargar las imagenes
                PdfImage logo1 = PdfImage.FromFile(images[1]);
                PdfImage logo2 = PdfImage.FromFile(images[0]);
                
                //Texto del guid
                page.Canvas.DrawString(dto.Id.ToString(), font, new PdfSolidBrush(Color.Black), 10, 350);

                
                //transformar el QR a imagen
                MemoryStream img = new MemoryStream();

                //una variable temporal que asimila el valor mas cercano para centrar el gráfico de QR
                var tempwidth = float.Parse((79.4).ToString()) / 2;

                //dibujar las imagenes
                page.Canvas.DrawImage(logo1, new PointF(10,10), new SizeF(120,50));
                page.Canvas.DrawImage(logo2, new PointF(165, 10), new SizeF(120, 50));

                PdfImage image;
                using (Bitmap bitMap = qrCode.GetGraphic(10))
                {
                    Bitmap resized = new Bitmap(bitMap, new Size(290, 290));

                    resized.Save(img, ImageFormat.Png);

                    image = PdfImage.FromStream(img);

                    
                }

                page.Canvas.DrawImage(image, new PointF(tempwidth, 100));
                doc.SaveToStream(ms);
                doc.Close();
                return File(ms.ToArray(), "application/pdf", "qr_exportado.pdf");

            }
        }

        private string[] GetPath()
        {
            string path = Server.MapPath("~/Images/");
            string[] images = Directory.GetFiles(path);
            return images;
        }
    }
}