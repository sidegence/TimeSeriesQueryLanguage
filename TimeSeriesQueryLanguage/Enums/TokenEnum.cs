using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSeriesQueryLanguage.Enums
{
    public enum TokenEnum
    {
        EOF,
        OpenParens,
        CloseParens,
        Comma,
        Number,
        Mult, Div, Add,
        And, Or,
        Agg, 
        FId,
        AggFn,
        AggCl,
        AggTsFr,
        AggTsTo,
        V1mV2,
        V1lV2,
        Scale,
        V1inV2V3,
    }
}
