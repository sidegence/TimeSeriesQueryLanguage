using FluentAssertions;
using TimeSeriesQueryLanguage.Core;
using TimeSeriesQueryLanguage.Samples.ClientEvalImplementations;

namespace TimeSeriesQueryLanguage.Tests
{
    public class AlgebraicNodeTests : BaseTest
    {
        [Test]
        [Repeat(1000)]
        public async Task UnitaryTests()
        {
            var p1 = new decimal(Random.Next(0, 2) == 0 ? 0 : Math.Pow(-1, Random.Next(1, 10)) * Random.NextDouble());
            string s = $"({p1})";
            var e = await Parser.Set(s).Parse()!.Eval(I42);
            Console.WriteLine($"{s} : {e}");
            e.Should().Be(p1);
        }

        [Test]
        [Repeat(1000)]
        [TestCase("+")]
      //[TestCase("-")] Not Implement yet or ever :(
        [TestCase("*")]
        [TestCase("/")]
        public async Task OperatorsWith2ArgumentsTests(string op)
        {
            var p1 = new decimal(Random.Next(0, 2) == 0 ? 0 : Math.Pow(-1, Random.Next(1, 10)) * Random.NextDouble());
            var p2 = new decimal(Random.Next(0, 2) == 0 ? 0 : Math.Pow(-1, Random.Next(1, 10)) * Random.NextDouble());
            string s = $"{op}({p1},{p2})";
            var e = await Parser.Set(s).Parse()!.Eval(I42);
            Console.WriteLine($"{s} : {e}");
            e.Should().Be(op switch
            {
                "+" => p1 + p2,
                "*" => p1 * p2,
                "/" => p2 == 0 ? 0 : p1 / p2,
                _ => throw new NotImplementedException()
            });
        }

        [Test]
        [Repeat(1000)]
        public async Task ComplexAlgebra_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturnCorrectEval()
        {
            var p1 = new decimal(Random.Next(0, 2) == 0 ? 0 : Math.Pow(-1, Random.Next(1, 10)) * Random.NextDouble());
            var p2 = new decimal(Random.Next(0, 2) == 0 ? 0 : Math.Pow(-1, Random.Next(1, 10)) * Random.NextDouble());
            string s = $"/({p1},+(*(*({p1},{p2}),*({p2},{p1})),{1000000}))";
            var e = await Parser.Set(s).Parse()!.Eval(I42);
            Console.WriteLine($"{s} : {e}");
            e.Should().Be((p1 / (((p1 * p2) * (p2 * p1) + 1000000))));
        }
    }
}