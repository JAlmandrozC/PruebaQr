using QRCoder;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
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

            return qrCode;
        }

        public Image GetImage(Uri uri)
        {
            WebClient client = new WebClient();
            Stream img = client.OpenRead(uri);
            Bitmap bitmap; bitmap = new Bitmap(img);

            Image image = (Image)bitmap;

            return image;
        }

        public dynamic SetColor(string color)
        {
            PdfSolidBrush brush = new PdfSolidBrush(Color.White);
            switch (color)
            {
                case "White":

                    brush = new PdfSolidBrush(Color.White);
                    break;

                case "Blue":
                    brush = new PdfSolidBrush(Color.Blue);
                    break;
                case "Gray":
                    brush = new PdfSolidBrush(Color.Gray);
                    break;
                case "LightGray":
                    brush = new PdfSolidBrush(Color.LightGray);
                    break;
                case "":
                    brush = new PdfSolidBrush(Color.White);
                    break;
            }

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
