using TimeSeriesQueryLanguage.Core;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;
using TimeSeriesQueryLanguage.Samples.ClientEvalImplementations;
using FluentAssertions;

namespace TimeSeriesQueryLanguage.Tests
{
    public class AlgebraicNodeTests
    {
        [Test]
        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(1.09)]
        [TestCase(0.09)]
        [TestCase(-1.09)]
        [Parallelizable(ParallelScope.All)]
        public void Number_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturnEntryArgument(decimal p1)
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();
            string fn = $"{p1}";

            var result = new TimeSeriesQueryLanguageParser().Set(fn)?.Parse()?.Eval(clientEvalImplementation).Result;

            result.Should().Be(p1);
        }

        [Test]
        [TestCase(0, 5)]
        [TestCase(5, 0)]
        [TestCase(1, -1)]
        [TestCase(0.56, 25)]
        [TestCase(345.5, 0.12)]
        [TestCase(112, -1.78)]
        [Parallelizable(ParallelScope.All)]
        public void Multiplication_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturnCorrectEval(decimal p1, decimal p2)
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();
            string fn = $"*({p1},{p2})";

            var result = new TimeSeriesQueryLanguageParser().Set(fn)?.Parse()?.Eval(clientEvalImplementation).Result;

            result.Should().Be(p1 * p2);
        }

        [Test]
        [TestCase(0, 5)]
        [TestCase(5, 0)]
        [TestCase(1, -1)]
        [TestCase(0.56, 25)]
        [TestCase(345.5, 0.12)]
        [TestCase(112, -1.78)]
        [Parallelizable(ParallelScope.All)]
        public void Division_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturnCorrectEval(decimal p1, decimal p2)
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();
            string fn = $"/({p1},{p2})";

            var result = new TimeSeriesQueryLanguageParser().Set(fn)?.Parse()?.Eval(clientEvalImplementation).Result;

            result.Should().Be(p2 == 0 ? 0 : p1 / p2);
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(9, 1)]
        [TestCase(100, 0)]
        [TestCase(1821, 198219)]
        [TestCase(0.9, 51)]
        [TestCase(3.9, 1.6)]
        [TestCase(10.0, -0.6)]
        [TestCase(182.1, -1.98219)]
        [TestCase(0, -1)]
        [TestCase(-9, -1)]
        [TestCase(-100, 0)]
        [TestCase(-1821, -198219)]
        [TestCase(-0.9, 51)]
        [TestCase(-3.9, -1.6)]
        [TestCase(-10.0, 0.6)]
        [TestCase(182.1, -1.98219)]
        [Parallelizable(ParallelScope.All)]
        public void ComplexAlgebra_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturnCorrectEval(decimal p1, decimal p2)
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();
            string fn = $"/({p1},+(*(*({p1},{p2}),*({p2},{p1})),{1000000}))";

            var result = new TimeSeriesQueryLanguageParser().Set(fn)?.Parse()?.Eval(clientEvalImplementation).Result;

            result.Should().Be((p1 / (((p1 * p2) * (p2 * p1) + 1000000))));
        }
    }
}