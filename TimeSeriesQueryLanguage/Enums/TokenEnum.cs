namespace TimeSeriesQueryLanguage.Enums
{
    public enum TokenEnum
    {
        EOF,
        OpenParens,
        CloseParens,
        Comma,
        Number,
        Mult, Div, Add,
        And, Or, NotEqual, Equal,
        Agg, 
        FId,
        AggFn,
        AggCl,
        AggTsFr,
        AggTsTo,
        V1mV2,
        V1lV2,
        Scale,
        V1inV2V3,
        V1V2V3Inc,
        V1V2V3Dec,
        HourOfDay,
        DayOfWeek,
        DayOfMonth,
        MonthOfYear,
    }
}
