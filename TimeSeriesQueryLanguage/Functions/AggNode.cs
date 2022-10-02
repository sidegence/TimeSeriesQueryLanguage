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
        readonly AggTimeIntervalEnum AggTsSlideTo;
        readonly AggTimeIntervalEnum AggTsFrame;
        public AggNode(TAggFn? aggFn, TAggCl? aggCl, AggTimeIntervalEnum aggTsSlideTo, AggTimeIntervalEnum aggTsFrame)
        {
            AggFn = aggFn; AggCl = aggCl; AggTsSlideTo = aggTsSlideTo; AggTsFrame = aggTsFrame;
        }
        public override Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            return ctx.Eval<TAggFn, TAggCl>(OperationEnum.Agg, AggFn, AggCl, AggTsSlideTo, AggTsFrame);
        }
    }
}
