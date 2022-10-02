using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public class FIdNode<TAggFn, TAggCl> : AbstractNode where TAggFn : Enum where TAggCl : Enum
    {
        readonly int Id;
        public FIdNode(decimal id)
        {
            Id = (int)id;
        }
        public override Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            return ctx.Eval<TAggFn, TAggCl>(OperationEnum.Fid, i: Id);
        }
    }
}
