using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public class TidNode<TAggFn, TAggCl> : AbstractNode where TAggFn : Enum where TAggCl : Enum
    {
        readonly TAggFn? AggFn;
        readonly decimal Tid;
        readonly AggTs AggTsSlideTo;
        readonly AggTs AggTsFrame;
        public TidNode(TAggFn? aggFn, decimal tid, AggTs aggTsSlideTo, AggTs aggTsFrame)
        {
            AggFn = aggFn; Tid = tid; AggTsSlideTo = aggTsSlideTo; AggTsFrame = aggTsFrame;
        }
        public override Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            return ctx.Eval<TAggFn, TAggCl>(default(TAggFn)/*.Tid*/, default(TAggCl), AggTsSlideTo, AggTsFrame, AggFn, (int) Tid);
        }
    }
}
