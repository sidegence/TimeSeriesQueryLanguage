using TimeSeriesQueryLanguage.Interfaces;

namespace TimeSeriesQueryLanguage.Functions
{
    public class HourOfDayNode : AbstractNode
    {
        public HourOfDayNode()
        {
        }
        public override async Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            return await Task.FromResult((decimal)DateTime.UtcNow.Hour);
        }
    }

    public class DateTimeNode : AbstractNode
    {
        public DateTimeNode()
        {
        }
        public override async Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            return await Task.FromResult((decimal)DateTime.UtcNow.DayOfWeek);
        }
    }

    public class DayOfMonthNode : AbstractNode
    {
        public DayOfMonthNode()
        {
        }
        public override async Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            return await Task.FromResult((decimal)DateTime.UtcNow.Day);
        }
    }

    public class MonthOfYearNode : AbstractNode
    {
        public MonthOfYearNode()
        {
        }
        public override async Task<decimal> Eval(ITimeSeriesQueryLanguageContext ctx)
        {
            return await Task.FromResult((decimal)DateTime.UtcNow.Month);
        }
    }
}
