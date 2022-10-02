
# TimeSeries Query Language

This is a library to aggregate values on user defined time series datasets.

## Quick Start

```
public class YOUREvalImplementation : ITimeSeriesQueryLanguageContext
{
    readonly YOURDbContext Db;
    public YOUREvalImplementation(YOURDbContext db)
    {
        Db = db;
    }
    public async Task<decimal> Eval(string fn)
    {
        var tsqlp = new TimeSeriesQueryLanguageParser().Set(fn)?.Parse();
        return tsqlp == null ? 0.0m : await tsqlp.Eval(this);
    }

    public async Task<decimal> Eval<TAggFn, TAggCl>(
        OperationEnum operationEnum = OperationEnum.Agg,
        TAggFn? aggFn = default,
        TAggCl? aggCl = default,
        AggTimeIntervalEnum aggTsSlideTo = AggTimeIntervalEnum.M0,
        AggTimeIntervalEnum aggTsFrame = AggTimeIntervalEnum.M0,
        int i = 0
    ) where TAggFn : Enum where TAggCl : Enum
    {
        if (aggFn == null || !Enum.IsDefined(typeof(TAggFn), aggFn) || aggCl == null || !Enum.IsDefined(typeof(TAggCl), aggCl))
            throw new ArgumentNullException("Eval<TAggFn, TAggCl> type arguments cannot be null or undefined");

        var tsSlideTo = (await Db.Tickers.FirstAsync()).ts - AggTsToTimeSpanMapping.Map(aggTsSlideTo);
        var tsFrameMin = tsSlideTo - AggTsToTimeSpanMapping.Map(aggTsFrame);
        var tickers = Db.Tickers.Where(_ => _.ts <= tsSlideTo && _.ts >= tsFrameMin);

        switch (Helper.Convert<AggregateFunctionsEnum>(aggFn.ToString()))
        {
            case AggFn.Cnt: return await tickers.CountAsync();
        }
        return 0.0m;
    }
}

// Average column price for the last day, starting now
var c1 = await YOUREvalImplementation.Eval("ag(Avg,price,To.M0,Fr.D1)");										

// Count all rows in data
var c2 = await YOUREvalImplementation.Eval<AggregateFunctionsEnum, AggregateColumnsEnum>(aggFn: AggregateFunctionsEnum.Cnt));

// Count rows in data for the last 5 mins, starting now 
var c3 = await YOUREvalImplementation.Eval("ag(Cnt,price,To.M0,Fr.M5)");										

// Count rows in data for the last 5 mins, starting 1 hour ago 
var c4 = await YOUREvalImplementation.Eval("ag(Cnt,price,To.H1,Fr.M5)");										
```

Open the Samples project to see it in action with many more examples.

The Eval function:
This function lives within your class implementation, that also holds a context to your data.
You can query your data with aggregates, algebraic and logical operators.

## Usage

The engine will interpret the function syntax, validate it and recall the Eval function on you implementation class, recursing all calls.

Aggregate Operators: ag
Algebraic Operators: +, *, /, sc
Logical Operators: &, |, <, >, in

## Support

For support, email sidegence@gmail.com


## Optimizations
