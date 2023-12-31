using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Samples.ClientEvalImplementations
{
    public class EvalImplementationThatAlwaysReturns42 : ITimeSeriesQueryLanguageContext
    {
        public async Task<decimal> Eval<AggegrateFunctions, AggegrateColumns>(
            OperationEnum operation = OperationEnum.Agg,
            AggegrateFunctions? aggFn = default,
            AggegrateColumns? aggCl = default, 
            AggTimeIntervalEnum aggTsFr = AggTimeIntervalEnum.Zero, 
            AggTimeIntervalEnum aggTsTo = AggTimeIntervalEnum.Zero,
            int i = 0
        ) where AggegrateFunctions : Enum where AggegrateColumns : Enum
        {
            var to = DateTime.UtcNow - AggTimeIntervalEnumToTimeSpan.Map(aggTsTo);
            var fr = to - AggTimeIntervalEnumToTimeSpan.Map(aggTsFr);
            return await Task.FromResult(42);
        }
    }
}
