using TimeSeriesQueryLanguage.Core;
using TimeSeriesQueryLanguage.Samples.ClientEvalImplementations;

namespace TimeSeriesQueryLanguage.Tests;

public abstract class BaseTest
{
    protected EvalImplementationThatAlwaysReturns42 I42 = new EvalImplementationThatAlwaysReturns42();
    protected TimeSeriesQueryLanguageParser<AggregateFunctionsEnum, AggregateColumnsEnum> Parser = new TimeSeriesQueryLanguageParser<AggregateFunctionsEnum, AggregateColumnsEnum>();
    protected Random Random = new Random();
}
