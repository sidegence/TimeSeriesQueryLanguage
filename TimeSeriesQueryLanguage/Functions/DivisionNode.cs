using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public class DivisionNode : AbstractNode
    {
        readonly List<AbstractNode> Args;
        public DivisionNode(List<AbstractNode> args)
        {
            if (args.Count != 2)
                throw new Exception("DivisionNode 2 args");
            Args = args.ToList();
        }
        public override async Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            var arg0 = await Args[0].Eval(ctx);
            var arg1 = await Args[1].Eval(ctx);
            return arg1 == 0 ? 0 : arg0 / arg1;
        }
    }
}
