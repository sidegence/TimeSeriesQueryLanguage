using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;

namespace TimeSeriesQueryLanguage.Interfaces
{
    public interface ITimeSeriesQueryLanguageContext
    {
        Task<decimal> Eval(AggFn aggFn, AggCl aggCl = AggCl.Cl0, AggTs aggTsSlideTo = AggTs.M0, AggTs aggTsFrame = AggTs.M0, AggFn aggFn2 = AggFn.Cnt, int i = 0);
    }
}
