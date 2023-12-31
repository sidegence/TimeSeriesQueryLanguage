using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public class AggNode<TAggFn, TAggCl> : AbstractNode where TAggFn : Enum where TAggCl : Enum
    {
        readonly TAggFn? AggFn;
        readonly TAggCl? AggCl;
        readonly AggTimeIntervalEnum AggTsFr;
        readonly AggTimeIntervalEnum AggTsTo;
        public AggNode(TAggFn? aggFn, TAggCl? aggCl, AggTimeIntervalEnum aggTsFr, AggTimeIntervalEnum aggTsTo)
        {
            AggFn = aggFn; AggCl = aggCl; AggTsFr = aggTsFr; AggTsTo = aggTsTo;
        }
        public override Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            return ctx.Eval<TAggFn, TAggCl>(OperationEnum.Agg, AggFn, AggCl, AggTsFr, AggTsTo);
        }
    }
}
