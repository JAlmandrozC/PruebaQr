using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaQr.Models
{
    public class FichaDto
    {
        public string Id { get; set; }

        //public string PreparedFor { get; set; }

        //public string ApprovedFor { get; set; }

        //public string DateFicha { get; set; }

        //public string Version { get; set; }
        public string EmpNom { get; set; }

        public Uri EmpImg { get; set; }

        

        //public Uri FichaImage { get; set; }

        public List<ItemsImprimirFichaDTo> Items { get; set; }
    }

    public class ItemsImprimirFichaDTo
    {

        public string CodUnc { get; set; }
        public string TipPro { get; set; }
        public string DescTPr {get; set;}
        public string ArtCod { get; set; }
        public string DscPro { get; set; }
        public string UniMed { get; set; }
        public string UbiArt { get; set; }
        public string RutPro { get; set; } //imagen ficha new Uri(string )

        public Int32 Cantid { get; set; }
    }
}