using TimeSeriesQueryLanguage.Core;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;
using TimeSeriesQueryLanguage.Samples.ClientEvalImplementations;
using FluentAssertions;
using System.Collections.Concurrent;

namespace TimeSeriesQueryLanguage.Tests
{
    public class AggegrationNodeTests
    {
        [Test]
        public async Task Agg_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturn42()
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();

            foreach (var aggFn in Enum.GetValues(typeof(AggregateFunctionsEnum)))
            {
                foreach (var aggCl in Enum.GetValues(typeof(AggregateColumnsEnum)))
                {
                    foreach (var aggTsFr in Enum.GetValues(typeof(AggTimeIntervalEnum)))
                    {
                        foreach (var aggTsTo in Enum.GetValues(typeof(AggTimeIntervalEnum)))
                        {
                            string fn = $"ag({aggFn},{aggCl},Fr.{aggTsFr},To.{aggTsTo})";
                            var expression = new TimeSeriesQueryLanguageParser<AggregateFunctionsEnum, AggregateColumnsEnum>().Set(fn);
                            Assert.NotNull(expression);

                            var parsing = expression.Parse();
                            Assert.NotNull(parsing);

                            var eval = await parsing.Eval(clientEvalImplementation);
                            eval.Should().Be(42);
                        }
                    }
                }
            }
        }
    }
}