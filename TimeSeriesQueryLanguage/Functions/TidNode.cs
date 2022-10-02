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
        readonly AggTimeIntervalEnum AggTsSlideTo;
        readonly AggTimeIntervalEnum AggTsFrame;
        public TidNode(TAggFn? aggFn, decimal tid, AggTimeIntervalEnum aggTsSlideTo, AggTimeIntervalEnum aggTsFrame)
        {
            AggFn = aggFn; Tid = tid; AggTsSlideTo = aggTsSlideTo; AggTsFrame = aggTsFrame;
        }
        public override Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            return ctx.Eval<TAggFn, TAggCl>(OperationEnum.Tid, AggFn, default(TAggCl), AggTsSlideTo, AggTsFrame, (int) Tid);
        }
    }
}
