﻿using System;
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
            OperationEnum operation = OperationEnum.Agg,
            AggegrateFunctions? aggFn = default,
            AggegrateColumns? aggCl = default, 
            AggTimeIntervalEnum aggTsSlideTo = AggTimeIntervalEnum.M0, 
            AggTimeIntervalEnum aggTsFrame = AggTimeIntervalEnum.M0,
            int i = 0
        ) where AggegrateFunctions : Enum where AggegrateColumns : Enum
        {
            return await Task.FromResult(42);
        }
    }
}
