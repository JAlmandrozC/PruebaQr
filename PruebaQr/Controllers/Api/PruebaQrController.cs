//using Spire.Pdf;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Mvc;

//namespace PruebaQr.Controllers.Api
//{
//    [RoutePrefix("api/pruebas")]
//    public class PruebaQrController : ApiController
//    {
//        [Route("qr")]
//        [HttpGet]
//        public async Task<System.Web.Mvc.ActionResult> QrExport(string modelName)
//        {
//            using (MemoryStream ms = new MemoryStream())
//            {
//                PdfDocument doc = new PdfDocument();
//                doc.SaveToStream(ms);
//                doc.Close();
//                //return View(ErrorViewName);
//                return File(ms.ToArray(), "application/pdf", "a.pdf");
//            }
//        }
//    }
//}
