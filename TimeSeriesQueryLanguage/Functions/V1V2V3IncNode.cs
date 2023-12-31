using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public class V1V2V3IncNode : AbstractNode
    {
        readonly List<AbstractNode> Args;
        public V1V2V3IncNode(List<AbstractNode> args)
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
            return V1V2V3Inc(arg0, arg1, arg2);
        }
        public static decimal V1V2V3Inc(decimal v1, decimal v2, decimal v3)
        {
            return v1 < v2 && v2 < v3 ? 1 : 0;
        }
    }
}
