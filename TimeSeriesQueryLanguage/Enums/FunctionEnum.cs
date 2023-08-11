using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSeriesQueryLanguage.Enums
{
    public enum FunctionEnum
    {
        Number,
        Agg,
    }
    public enum ArgFunctionEnum
    {
        And, Or,
        V1mV2,
        V1lV2,
        V1inV2V3,
        Function,
    }
}
