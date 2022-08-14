using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public class AddNode : AbstractNode
    {
        readonly List<AbstractNode> Args;
        public AddNode(List<AbstractNode> args)
        {
            if (args.Count != 2)
                throw new Exception("needs 2 args");
            Args = args.ToList();
        }
        public override async Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            var arg0 = await Args[0].Eval(ctx);
            var arg1 = await Args[1].Eval(ctx);
            return arg0 + arg1;
        }
    }

}
