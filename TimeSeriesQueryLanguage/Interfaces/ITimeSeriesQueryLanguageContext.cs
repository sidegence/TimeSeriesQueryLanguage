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
        Task<decimal> Eval<TAggFn, TAggCl>(
            TAggFn? aggFn = default,
            TAggCl? aggCl = default, 
            AggTs aggTsSlideTo = AggTs.M0, 
            AggTs aggTsFrame = AggTs.M0,
            TAggFn? aggFn2 = default,
            int i = 0
        ) where TAggFn : Enum where TAggCl : Enum;
    }
}
