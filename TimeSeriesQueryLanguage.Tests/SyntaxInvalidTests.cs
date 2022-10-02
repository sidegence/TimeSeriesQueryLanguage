using TimeSeriesQueryLanguage.Core;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;
using TimeSeriesQueryLanguage.Samples.ClientEvalImplementations;
using FluentAssertions;

namespace TimeSeriesQueryLanguage.Tests
{
    public class SyntaxInvalidTests
    {
        [Test]
        [TestCase("*[1,2]")]
        [TestCase("*()")]
        [TestCase("*(1,")]
        [TestCase("a/")]
        [TestCase("AA")]
        [TestCase("/23)")]
        [TestCase("/(-(1,2),+(1,2))")]
        [Parallelizable(ParallelScope.All)]
        public void WhenUsingUncorrectSyntax_ShouldThrow(string fn)
        {
            FluentActions
                .Invoking(() => new TimeSeriesQueryLanguageParser<AggegrateFunctions>().Set(fn)?.Parse()?.Eval(new EvalImplementationThatAlwaysReturns42()).Result)
                .Should().Throw<Exception>();
        }
    }
}