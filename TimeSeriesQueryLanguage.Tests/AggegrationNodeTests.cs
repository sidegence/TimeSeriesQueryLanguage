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

            foreach (var aggFn in Enum.GetValues(typeof(AggFn)))
            {
                foreach (var aggCl in Enum.GetValues(typeof(AggCl)))
                {
                    foreach (var aggTsSlideTo in Enum.GetValues(typeof(AggTs)))
                    {
                        foreach (var aggTsFrame in Enum.GetValues(typeof(AggTs)))
                        {
                            string fn = $"ag({aggFn},{aggCl},To.{aggTsSlideTo},Fr.{aggTsFrame})";
                            var result = new TimeSeriesQueryLanguageParser().Set(fn)?.Parse()?.Eval(clientEvalImplementation).Result;
                            result.Should().Be(42);
                        }
                    }
                }
            }
        }
    }
}