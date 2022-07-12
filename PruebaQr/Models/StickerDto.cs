using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaQr.Models
{
    public class StickerDto
    {



    }

    public class ItemsImprimirDTo
    {
        public int NroItm { get; set; }
        public string Deposi { get; set; }
        public string Sector { get; set; }
        public string TipPro { get; set; }
        public string ArtCod { get; set; }
        public string Descrp { get; set; }
        public string UniMed { get; set; }
        public double Cantid { get; set; }
        public string UbiArt { get; set; }
    }

    public class QrDemandaDTo
    {
        public string Id { get; set; }
        public string RazonSocial { get; set; }
        public List<ItemsImprimirDTo> ItemsImprimirDTo { get; set; }
    }
}