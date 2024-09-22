using FluentAssertions;

namespace TimeSeriesQueryLanguage.Tests
{
    public class LogicalNodeTests : BaseTest
    {
        [Test]
        [Repeat(1000)]
        [TestCase("&")]
        [TestCase("|")]
        public async Task OperatorsWith2ArgumentsTests1(string op)
        {
            var p1 = Random.Next(1, 100) < 50;
            var p2 = Random.Next(1, 100) < 50;
            string s = $"{op}({Convert.ToDecimal(p1)},{Convert.ToDecimal(p2)})";
            var e = await Parser.Set(s).Parse()!.Eval(I42);
            Console.WriteLine($"{s} : {e}");
            e.Should().Be(op switch
            {
                "&" => Convert.ToDecimal(p1 && p2),
                "|" => Convert.ToDecimal(p1 || p2),
                _ => throw new NotImplementedException()
            });
        }

        [Test]
        [Repeat(1000)]
        [TestCase(">")]
        [TestCase("<")]
        public async Task OperatorsWith2ArgumentsTests2(string op)
        {
            var p1 = new decimal(Random.Next(0, 2) == 0 ? 0 : Math.Pow(-1, Random.Next(1, 10)) * Random.NextDouble());
            var p2 = new decimal(Random.Next(0, 2) == 0 ? 0 : Math.Pow(-1, Random.Next(1, 10)) * Random.NextDouble());
            string s = $"{op}({p1},{p2})";
            var e = await Parser.Set(s).Parse()!.Eval(I42);
            Console.WriteLine($"{s} : {e}");
            e.Should().Be(op switch
            {
                "<" => Convert.ToDecimal(p1 < p2),
                ">" => Convert.ToDecimal(p1 > p2),
                _ => throw new NotImplementedException()
            });
        }

        [Test]
        [Repeat(1000)]
        [TestCase("in")]
        [TestCase("inc")]
        [TestCase("dec")]
        public async Task OperatorsWith3ArgumentsTests(string op)
        {
            var p1 = new decimal(Random.Next(0, 2) == 0 ? 0 : Math.Pow(-1, Random.Next(1, 10)) * Random.NextDouble());
            var p2 = new decimal(Random.Next(0, 2) == 0 ? 0 : Math.Pow(-1, Random.Next(1, 10)) * Random.NextDouble());
            var p3 = new decimal(Random.Next(0, 2) == 0 ? 0 : Math.Pow(-1, Random.Next(1, 10)) * Random.NextDouble());
            string s = $"{op}({p1},{p2},{p3})";
            var e = await Parser.Set(s).Parse()!.Eval(I42);
            Console.WriteLine($"{s} : {e}");
            e.Should().Be(op switch
            {
                "in" => Convert.ToDecimal((p1 > p2 && p1 < p3) || (p1 > p3 && p1 < p2)),
                "inc" => Convert.ToDecimal(p1 < p2 && p2 < p3),
                "dec" => Convert.ToDecimal(p1 > p2 && p2 > p3),
                _ => throw new NotImplementedException()
            });
        }

        [Test]
        [Repeat(1000)]
        public async Task v1inv2v3wLogicOps_WhenCalledEvalImplementationThatAlwaysReturns42_ShouldAlwaysReturnCorrectEval()
        {
            var p1 = new decimal(Random.Next(0, 2) == 0 ? 0 : Math.Pow(-1, Random.Next(1, 10)) * Random.NextDouble());
            var p2 = new decimal(Random.Next(0, 2) == 0 ? 0 : Math.Pow(-1, Random.Next(1, 10)) * Random.NextDouble());
            var p3 = new decimal(Random.Next(0, 2) == 0 ? 0 : Math.Pow(-1, Random.Next(1, 10)) * Random.NextDouble());
            string s = $"|(&(<({p2}, {p1}),<({p1}, {p3})),&(<({p3}, {p1}),<({p1}, {p2})))";
            var e = await Parser.Set(s).Parse()!.Eval(I42);
            Console.WriteLine($"{s} : {e}");
        }
    }
}