using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public class AggNode<TAggFn, TAggCl> : AbstractNode where TAggFn : Enum where TAggCl : Enum
    {
        readonly TAggFn? AggFn;
        readonly TAggCl? AggCl;
        readonly AggTs AggTsSlideTo;
        readonly AggTs AggTsFrame;
        public AggNode(TAggFn? aggFn, TAggCl? aggCl, AggTs aggTsSlideTo, AggTs aggTsFrame)
        {
            AggFn = aggFn; AggCl = aggCl; AggTsSlideTo = aggTsSlideTo; AggTsFrame = aggTsFrame;
        }
        public override Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            return ctx.Eval<TAggFn, TAggCl>(AggFn, AggCl, AggTsSlideTo, AggTsFrame);
        }
    }
}
