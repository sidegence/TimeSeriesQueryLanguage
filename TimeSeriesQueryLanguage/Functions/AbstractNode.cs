using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public abstract class AbstractNode
    {
        public abstract Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx);
    }
}
