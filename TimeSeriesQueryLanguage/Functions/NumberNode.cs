using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public class NumberNode : AbstractNode
    {
        readonly decimal Number;
        public NumberNode(decimal number)
        {
            Number = number;
        }
        public override async Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            return await Task.FromResult(Number);
        }
    }
}
