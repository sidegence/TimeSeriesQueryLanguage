using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSeriesQueryLanguage.Samples.Persistence
{
    public class Ticker
    {
        [Key]
        public int id { get; set; }
        public DateTime ts { get; set; }
        public decimal price { get; set; }
        public decimal qty { get; set; }
        public string side { get; set; } = "";
    }
}
