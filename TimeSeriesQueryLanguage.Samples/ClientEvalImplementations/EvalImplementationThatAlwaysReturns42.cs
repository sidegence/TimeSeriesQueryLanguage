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
        public async Task<decimal> Eval<AggegrateFunctions, AggegrateColumns>(
            AggegrateFunctions? aggFn,
            AggegrateColumns? aggCl = default, 
            AggTs aggTsSlideTo = AggTs.M0, 
            AggTs aggTsFrame = AggTs.M0,
            AggegrateFunctions? aggFn2 = default,
            int i = 0
        ) where AggegrateFunctions : Enum where AggegrateColumns : Enum
        {
            return await Task.FromResult(42);
        }
    }
}
