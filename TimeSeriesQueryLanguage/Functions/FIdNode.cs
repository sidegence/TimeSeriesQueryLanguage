using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public class FIdNode : AbstractNode
    {
        readonly int Id;
        public FIdNode(decimal id)
        {
            Id = (int)id;
        }
        public override Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            return ctx.Eval(AggFn.FId, i: Id);
        }
    }
}
