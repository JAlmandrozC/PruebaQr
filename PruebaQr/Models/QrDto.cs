using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaQr.Models
{
    public class QrDto
    {
        public Guid Id { get; set; }

        public Uri ImageUrl1 { get; set; }

        public Uri ImageUrl2 { get; set; }

        public string ColorBody { get; set; }

        public string ColorFooter { get; set; }

        public string Plate { get; set; }

        public string Label1 { get; set; }

        public string Label2 { get; set; }
        public string TextFooter1 { get; set; }
        public string TextFooter2 { get; set; }
    }
}