using FluentAssertions;
using TimeSeriesQueryLanguage.Core;
using TimeSeriesQueryLanguage.Samples.ClientEvalImplementations;

namespace TimeSeriesQueryLanguage.Tests
{
    public class DateTimeNodeTests : BaseTest
    {
        [Test]
        public async Task HourOfDayTests()
        {
            var e = await Parser.Set("hod").Parse()!.Eval(I42);
            Console.WriteLine($"{e}");
            e.Should().Be((decimal)DateTime.UtcNow.Hour);
        }

        [Test]
        public async Task DayOfWeekTests()
        {
            var e = await Parser.Set("dow").Parse()!.Eval(I42);
            Console.WriteLine($"{e}");
            e.Should().Be((decimal)DateTime.UtcNow.DayOfWeek);
        }

        [Test]
        public async Task DayOfMonthTests()
        {
            var e = await Parser.Set("dom").Parse()!.Eval(I42);
            Console.WriteLine($"{e}");
            e.Should().Be((decimal)DateTime.UtcNow.Day);
        }

        [Test]
        public async Task MonthOfYearTests()
        {
            var e = await Parser.Set("moy").Parse()!.Eval(I42);
            Console.WriteLine($"{e}");
            e.Should().Be((decimal)DateTime.UtcNow.Month);
        }
    }
}