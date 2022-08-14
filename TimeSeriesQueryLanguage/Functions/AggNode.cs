using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public class AggNode : AbstractNode
    {
        readonly AggFn AggFn;
        readonly AggCl AggCl;
        readonly AggTs AggTsSlideTo;
        readonly AggTs AggTsFrame;
        public AggNode(AggFn aggFn, AggCl aggCl, AggTs aggTsSlideTo, AggTs aggTsFrame)
        {
            AggFn = aggFn; AggCl = aggCl; AggTsSlideTo = aggTsSlideTo; AggTsFrame = aggTsFrame;
        }
        public override Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            return ctx.Eval(AggFn, AggCl, AggTsSlideTo, AggTsFrame);
        }
    }
}
