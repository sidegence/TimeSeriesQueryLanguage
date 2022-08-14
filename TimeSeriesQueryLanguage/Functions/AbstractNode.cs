using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public abstract class AbstractNode
    {
        public abstract Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx);
    }
}
