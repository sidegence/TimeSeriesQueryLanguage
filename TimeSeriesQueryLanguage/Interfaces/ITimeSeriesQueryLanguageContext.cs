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
            OperationEnum operationEnum = OperationEnum.Agg,
            TAggFn? aggFn = default,
            TAggCl? aggCl = default, 
            AggTimeIntervalEnum aggTsSlideTo = AggTimeIntervalEnum.M0, 
            AggTimeIntervalEnum aggTsFrame = AggTimeIntervalEnum.M0,
            int i = 0
        ) where TAggFn : Enum where TAggCl : Enum;
    }
}
