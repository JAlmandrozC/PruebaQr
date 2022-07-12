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

        public byte RedBody { get; set; }
        public byte GreenBody { get; set; }
        public byte BlueBody { get; set; }

        public byte RedFooter { get; set; }
        public byte GreenFooter { get; set; }
        public byte BlueFooter { get; set; }

        public byte RedText { get; set; }
        public byte GreenText { get; set; }
        public byte BlueText { get; set; }

        public string Plate { get; set; }

        public string Label1 { get; set; }

        public string Label2 { get; set; }
        public string TextFooter1 { get; set; }
        public string TextFooter2 { get; set; }
    }
}