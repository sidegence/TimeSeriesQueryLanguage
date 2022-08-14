using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public class V1lV2Node : AbstractNode
    {
        readonly List<AbstractNode> Args;
        public V1lV2Node(List<AbstractNode> args)
        {
            if (args.Count != 2)
                throw new Exception("needs 2 args");
            Args = args.ToList();
        }
        public override async Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            var arg0 = await Args[0].Eval(ctx);
            var arg1 = await Args[1].Eval(ctx);
            return V1lV2(arg0, arg1);
        }
        private decimal V1lV2(decimal v1, decimal v2)
        {
            return (v1 < v2 ? 1 : 0);
        }
    }

}
