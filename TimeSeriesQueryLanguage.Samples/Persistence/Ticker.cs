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

        public string Title()
        {
            return $"{"Id",4} {"Ts",19} {"Price",6} {"Qty",6} {"B/S",1}";
        }
        public override string ToString()
        {
            return $"{id,4} {ts,20} {price,6} {qty,6} {side,1}";
        }
    }
}
