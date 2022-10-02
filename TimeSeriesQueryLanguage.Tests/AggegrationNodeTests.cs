using TimeSeriesQueryLanguage.Core;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;
using TimeSeriesQueryLanguage.Samples.ClientEvalImplementations;
using FluentAssertions;

namespace TimeSeriesQueryLanguage.Tests
{
    public class AggegrationNodeTests
    {
        [Test]
        public void Agg_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturn42()
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();

            foreach (var aggFn in Enum.GetValues(typeof(AggregateFunctionsEnum)))
            {
                foreach (var aggCl in Enum.GetValues(typeof(AggregateColumnsEnum)))
                {
                    foreach (var aggTsSlideTo in Enum.GetValues(typeof(AggTimeIntervalEnum)))
                    {
                        foreach (var aggTsFrame in Enum.GetValues(typeof(AggTimeIntervalEnum)))
                        {
                            string fn = $"ag({aggFn},{aggCl},To.{aggTsSlideTo},Fr.{aggTsFrame})";
                            var result = new TimeSeriesQueryLanguageParser<AggregateFunctionsEnum, AggregateColumnsEnum>().Set(fn)?.Parse()?.Eval(clientEvalImplementation).Result;
                            result.Should().Be(42);
                        }
                    }
                }
            }
        }
    }
}