using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSeriesQueryLanguage.Enums
{
    public enum Token
    {
        EOF,
        OpenParens,
        CloseParens,
        Comma,
        Number,
        Mult, Div, Add,
        And, Or,
        Agg, 
        Tid,
        FId,
        AggFn,
        AggCl,
        AggTsSlideTo,
        AggTsFrame,
        V1mV2,
        V1lV2,
        Scale,
        V1inV2V3,
    }
}
