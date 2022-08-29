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
    public class TidNode : AbstractNode
    {
        readonly AggFn AggFn;
        readonly decimal Tid;
        readonly AggTs AggTsSlideTo;
        readonly AggTs AggTsFrame;
        public TidNode(AggFn aggFn, decimal tid, AggTs aggTsSlideTo, AggTs aggTsFrame)
        {
            AggFn = aggFn; Tid = tid; AggTsSlideTo = aggTsSlideTo; AggTsFrame = aggTsFrame;
        }
        public override Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            return ctx.Eval(AggFn.Tid, AggCl.Cl0, AggTsSlideTo, AggTsFrame, AggFn, (int) Tid);
        }
    }
}
