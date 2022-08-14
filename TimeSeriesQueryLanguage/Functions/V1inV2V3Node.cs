using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public class V1inV2V3Node : AbstractNode
    {
        readonly List<AbstractNode> Args;
        public V1inV2V3Node(List<AbstractNode> args)
        {
            if (args.Count != 3)
                throw new Exception("needs 3 args");
            Args = args.ToList();
        }
        public override async Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            var arg0 = await Args[0].Eval(ctx);
            var arg1 = await Args[1].Eval(ctx);
            var arg2 = await Args[2].Eval(ctx);
            return V1InV2V3(arg0, arg1, arg2);
        }
        public static decimal V1InV2V3(decimal v1, decimal v2, decimal v3)
        {
            return v2 == v3 ? 0.0m : ((v1 > v2 && v1 < v3) || (v1 > v3 && v1 < v2) ? 1 : 0);
        }
    }
}
