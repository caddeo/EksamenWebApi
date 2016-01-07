using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiEksamenForberedelse.Models
{
    public class Auktionsvarer
    {
        public string Varenummer { get; set; }
        public string Varebetegnelse { get; set; }
        public int Vurdering { get; set; }
        public int BudPris { get; set; }
        public string BudKundeNavn { get; set; }
        public string BudKundeTelefon { get; set; }
        public DateTime BudTidspunkt { get; set; }
    }
}