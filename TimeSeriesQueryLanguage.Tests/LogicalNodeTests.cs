using FluentAssertions;
using TimeSeriesQueryLanguage.Core;
using TimeSeriesQueryLanguage.Samples.ClientEvalImplementations;

namespace TimeSeriesQueryLanguage.Tests
{
    public class LogicalNodeTests
    {
        [Test]
        [TestCase(0, 5)]
        [TestCase(5, 0)]
        [TestCase(1, -1)]
        [TestCase(1, 1)]
        [Parallelizable(ParallelScope.All)]
        public void And_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturnCorrectEval(decimal p1, decimal p2)
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();
            string fn = $"&({p1},{p2})";
            var result = new TimeSeriesQueryLanguageParser<AggregateFunctionsEnum,AggregateColumnsEnum>().Set(fn)?.Parse()?.Eval(clientEvalImplementation).Result;
            result.Should().Be(Convert.ToDecimal(Convert.ToBoolean(p1) && Convert.ToBoolean(p2)));
        }

        [Test]
        [TestCase(0, 5)]
        [TestCase(5, 0)]
        [TestCase(1, -1)]
        [TestCase(1, 1)]
        [Parallelizable(ParallelScope.All)]
        public void Or_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturnCorrectEval(decimal p1, decimal p2)
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();
            string fn = $"|({p1},{p2})";
            var result = new TimeSeriesQueryLanguageParser<AggregateFunctionsEnum, AggregateColumnsEnum>().Set(fn)?.Parse()?.Eval(clientEvalImplementation).Result;
            result.Should().Be(Convert.ToDecimal(Convert.ToBoolean(p1) || Convert.ToBoolean(p2)));
        }

        [Test]
        [TestCase(5, 5)]
        [TestCase(10, 1)]
        [TestCase(1, 10)]
        [TestCase(-5.3, -5.3)]
        [TestCase(10.12, 1.1)]
        [TestCase(9.9999999999999999999, 9.99999999999999999991)]
        [Parallelizable(ParallelScope.All)]
        public void v1mv2_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturnCorrectEval(decimal p1, decimal p2)
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();
            string fn = $">({p1},{p2})";
            var result = new TimeSeriesQueryLanguageParser<AggregateFunctionsEnum, AggregateColumnsEnum>().Set(fn)?.Parse()?.Eval(clientEvalImplementation).Result;
            result.Should().Be(p1 > p2 ? 1 : 0);
        }

        [Test]
        [TestCase(5, 5)]
        [TestCase(10, 1)]
        [TestCase(1, 10)]
        [TestCase(-5.3, -5.3)]
        [TestCase(10.12, 1.1)]
        [TestCase(9.9999999999999999999, 9.99999999999999999991)]
        [Parallelizable(ParallelScope.All)]
        public void v1lv2_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturnCorrectEval(decimal p1, decimal p2)
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();
            string fn = $"<({p1},{p2})";
            var result = new TimeSeriesQueryLanguageParser<AggregateFunctionsEnum, AggregateColumnsEnum>().Set(fn)?.Parse()?.Eval(clientEvalImplementation).Result;
            result.Should().Be(p1 < p2 ? 1 : 0);
        }

        [Test]
        [TestCase(0, 0, 0, 0)]
        [TestCase(0, 1, 100, 0)]
        [TestCase(1, 1, 100, 0)]
        [TestCase(100, 1, 100, 0)]
        [TestCase(9, 1, 100, 1)]
        [TestCase(0, 100, 1, 0)]
        [TestCase(1, 100, 1, 0)]
        [TestCase(100, 100, 1, 0)]
        [TestCase(9, 100, 1, 1)]
        [TestCase(0, -100, -1, 0)]
        [TestCase(1, -100, -1, 0)]
        [TestCase(100, -100, -1, 0)]
        [TestCase(-9, -100, -1, 1)]
        [Parallelizable(ParallelScope.All)]
        public void v1inv2v3_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturnCorrectEval(decimal p1, decimal p2, decimal p3, decimal expected)
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();
            string fn = $"in({p1}, {p2}, {p3})";
            var result = new TimeSeriesQueryLanguageParser<AggregateFunctionsEnum, AggregateColumnsEnum>().Set(fn)?.Parse()?.Eval(clientEvalImplementation).Result;
            result.Should().Be(expected);
        }

        [Test]
        [TestCase(0, 0, 0, 0)]
        [TestCase(0, 1, 100, 1)]
        [TestCase(100, 1, 0, 0)]
        [TestCase(1, 1, 100, 0)]
        [TestCase(100, 1, 100, 0)]
        [TestCase(9, 1, 100, 0)]
        [TestCase(0, 100, 1, 0)]
        [TestCase(1, 100, 1, 0)]
        [TestCase(100, 100, 1, 0)]
        [TestCase(9, 100, 1, 0)]
        [TestCase(0, -100, -1, 0)]
        [TestCase(1, -100, -1, 0)]
        [TestCase(100, -100, -1, 0)]
        [TestCase(-9, -100, -1, 0)]
        [Parallelizable(ParallelScope.All)]
        public void v1v2v3inc_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturnCorrectEval(decimal p1, decimal p2, decimal p3, decimal expected)
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();
            string fn = $"inc({p1}, {p2}, {p3})";
            var result = new TimeSeriesQueryLanguageParser<AggregateFunctionsEnum, AggregateColumnsEnum>().Set(fn)?.Parse()?.Eval(clientEvalImplementation).Result;
            result.Should().Be(expected);
        }

        [Test]
        [TestCase(0, 0, 0, 0)]
        [TestCase(0, 1, 100, 0)]
        [TestCase(100, 1, 0, 1)]
        [TestCase(1, 1, 100, 0)]
        [TestCase(100, 1, 100, 0)]
        [TestCase(9, 1, 100, 0)]
        [TestCase(0, 100, 1, 0)]
        [TestCase(1, 100, 1, 0)]
        [TestCase(100, 100, 1, 0)]
        [TestCase(9, 100, 1, 0)]
        [TestCase(0, -100, -1, 0)]
        [TestCase(1, -100, -1, 0)]
        [TestCase(100, -100, -1, 0)]
        [TestCase(-9, -100, -1, 0)]
        [Parallelizable(ParallelScope.All)]
        public void v1v2v3dec_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturnCorrectEval(decimal p1, decimal p2, decimal p3, decimal expected)
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();
            string fn = $"dec({p1}, {p2}, {p3})";
            var result = new TimeSeriesQueryLanguageParser<AggregateFunctionsEnum, AggregateColumnsEnum>().Set(fn)?.Parse()?.Eval(clientEvalImplementation).Result;
            result.Should().Be(expected);
        }

        [Test]
        [TestCase(0, 0, 0, 0)]
        [TestCase(0, 1, 100, 0)]
        [TestCase(1, 1, 100, 0)]
        [TestCase(100, 1, 100, 0)]
        [TestCase(9, 1, 100, 1)]
        [TestCase(0, 100, 1, 0)]
        [TestCase(1, 100, 1, 0)]
        [TestCase(100, 100, 1, 0)]
        [TestCase(9, 100, 1, 1)]
        [TestCase(0, -100, -1, 0)]
        [TestCase(1, -100, -1, 0)]
        [TestCase(100, -100, -1, 0)]
        [TestCase(-9, -100, -1, 1)]
        [Parallelizable(ParallelScope.All)]
        public void v1inv2v3wLogicOps_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturnCorrectEval(decimal p1, decimal p2, decimal p3, decimal expected)
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();
            string fn = $"|(&(<({p2}, {p1}),<({p1}, {p3})),&(<({p3}, {p1}),<({p1}, {p2})))";
            var result = new TimeSeriesQueryLanguageParser<AggregateFunctionsEnum, AggregateColumnsEnum>().Set(fn)?.Parse()?.Eval(clientEvalImplementation).Result;
            result.Should().Be(expected);
        }
    }
}