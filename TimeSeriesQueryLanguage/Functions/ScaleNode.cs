using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public class ScaleNode : AbstractNode
    {
        readonly List<AbstractNode> Args;
        public ScaleNode(List<AbstractNode> args)
        {
            if (args.Count != 5)
                throw new Exception("needs 5 args");
            Args = args.ToList();
        }
        public override async Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            var arg0 = await Args[0].Eval(ctx);
            var arg1 = await Args[1].Eval(ctx);
            var arg2 = await Args[2].Eval(ctx);
            var arg3 = await Args[3].Eval(ctx);
            var arg4 = await Args[4].Eval(ctx);
            return V1OnScaleToScale(arg0, arg1, arg2, arg3, arg4);
        }
        public static decimal V1OnScaleToScale(decimal value, decimal scale1a, decimal scale1b, decimal scale2a, decimal scale2b)
        {
            if (scale1a == scale1b)
                return (scale2b - scale2a) / 2.0m;
            return (value - scale1a) * (scale2b - scale2a) / (scale1b - scale1a) + scale2a;
        }
    }

}
