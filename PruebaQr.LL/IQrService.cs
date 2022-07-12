using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using Spire.Pdf.Graphics;
using QRCoder;

namespace PruebaQr.LL
{
   public interface IQrService
    {
        dynamic CreateQr(Guid guid);

        void ResizeQr(PdfImage image);

        dynamic CreateCustomQr(string str);

        Stream ToStream(Image image);

        Image GetImage(Uri uri);

        dynamic SetColor(byte red,byte green, byte blue);

        dynamic AdjustQr(float w, float h, float pagew, float pageh);

        dynamic AdjustCustomX(float pagew,float w);
    }
}
