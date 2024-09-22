using FluentAssertions;
using TimeSeriesQueryLanguage.Core;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Samples.ClientEvalImplementations;

namespace TimeSeriesQueryLanguage.Tests
{
    public class AggegrationNodeTests : BaseTest
    {
        [Test]
        [Repeat(1)]
        public async Task Agg_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturn42()
        {
            foreach (var aggFn in Enum.GetValues(typeof(AggregateFunctionsEnum)))
            {
                foreach (var aggCl in Enum.GetValues(typeof(AggregateColumnsEnum)))
                {
                    foreach (var aggTsFr in Enum.GetValues(typeof(AggTimeIntervalEnum)))
                    {
                        foreach (var aggTsTo in Enum.GetValues(typeof(AggTimeIntervalEnum)))
                        {
                            string fn = $"ag({aggFn},{aggCl},Fr.{aggTsFr},To.{aggTsTo})";
                            var eval = await Parser.Set(fn).Parse()!.Eval(I42);
                            eval.Should().Be(42.0m);
                        }
                    }
                }
            }
        }
    }
}