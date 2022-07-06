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

        public string Color { get; set; }

        public string Plate { get; set; }
    }
}