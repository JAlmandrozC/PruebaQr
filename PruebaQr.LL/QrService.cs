using QRCoder;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace PruebaQr.LL
{
    public class QrService : IQrService
    {
        public QrService()
        {
                
        }

        public dynamic AdjustCustomX(float pagew, float w)
        {

            float x = (pagew / 2 - w) / 2;
            float xr = (pagew / 2) + x;

            return (x,xr);
        }

        public dynamic AdjustQr(float w, float h,float pagew, float pageh)
        {
            float width = w * 0.75f;

            float height = h * 0.75f;

            float x = (pagew - width) / 2;

            float y = (pageh - height) / 2;

            return (x,y,width,height);
        }

        public dynamic CreateCustomQr(string str)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(str, QRCodeGenerator.ECCLevel.L);
            QRCode qrCode = new QRCode(qrCodeData);

            return qrCode;
        }

        public dynamic CreateQr(Guid guid)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(guid.ToString(), QRCodeGenerator.ECCLevel.L);
            QRCode qrCode = new QRCode(qrCodeData);

            return (qrCode);
        }
        

        public Image GetImage(Uri uri)
        {
            WebClient client = new WebClient();
            Stream img = client.OpenRead(uri);
            Bitmap bitmap; bitmap = new Bitmap(img);

            Image image = (Image)bitmap;

            return image;
        }

        public dynamic GetQr(QRCode code)
        {
            return code;
        }

        public void ResizeQr(PdfImage image)
        {
            throw new NotImplementedException();
        }

        public dynamic SetColor(byte red, byte green, byte blue)
        {
            PdfSolidBrush brush = new PdfSolidBrush(new PdfRGBColor(red,green,blue));


            return brush;
        }

        public Stream ToStream(Image image)
        {
            var stream = new MemoryStream();

            image.Save(stream, image.RawFormat);
            stream.Position = 0;

            return stream;
        }

    }
}
