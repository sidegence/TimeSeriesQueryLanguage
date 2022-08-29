using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Samples.ClientEvalImplementations
{
    public class EvalImplementationThatAlwaysReturns42 : ITimeSeriesQueryLanguageContext
    {
        public async Task<decimal> Eval(AggFn aggFn, AggCl aggCl = AggCl.Cl0, AggTs aggTsSlideTo = AggTs.M0, AggTs aggTsFrame = AggTs.M0, AggFn aggFn2 = AggFn.Cnt, int i = 0)
        {
            return await Task.FromResult(42);
        }
    }
}
