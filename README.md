
# TimeSeries Query Language

This is a library to aggregate values on user defined time series datasets.

The syntax is simple yet powerfull because it can compare series of data.

    Example: "ag(Avg, Price, Fr.H1, To.Zero)"  means ''Get average price for the last hour''
    Example: ">(ag(Avg, Price, Fr.H1, To.Zero),ag(Avg, Price, Fr.D1, To.Zero))" means ''Is price average for the last hour bigger than the price average for he last 24h''

For flexibility Avg is a function defined by your own implementation.
On some application, its common to have Avg, Count, StandadrDeviation, Min, Max, etc 

And Price is again defined by your own dataset implementation. Tipically columns in tables.
On financial applications, its common to have Price, QuantityBought, QuantitySold, etc

The Eval method will always return a decimal.

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
        AggTimeIntervalEnum aggTsFr = AggTimeIntervalEnum.Zero,
        AggTimeIntervalEnum aggTsTo = AggTimeIntervalEnum.Zero,
        int i = 0
    ) where TAggFn : Enum where TAggCl : Enum
    {
        if (aggFn == null || !Enum.IsDefined(typeof(TAggFn), aggFn) || aggCl == null || !Enum.IsDefined(typeof(TAggCl), aggCl))
            throw new ArgumentNullException("Eval<TAggFn, TAggCl> type arguments cannot be null or undefined");

        var to = (await Db.Tickers.FirstAsync()).ts - AggTsToTimeSpanMapping.Map(aggTsTo);
        var fr = to - AggTsToTimeSpanMapping.Map(aggTsFr);
        var tickers = Db.Tickers.Where(_ => _.ts >= fr && _.ts <= to);

        switch (Helper.Convert<AggregateFunctionsEnum>(aggFn.ToString()))
        {
            case AggFn.Cnt: return await tickers.CountAsync();
        }
        return 0.0m;
    }
}

// Average column price for the last day (D1), starting now (Zero)
var c1 = await YOUREvalImplementation.Eval("ag(Avg,price,Fr.D1,To.Zero)");										

```

Open the Samples project to see it in action with many more examples.

The Eval function:
This function lives within your class implementation, that also holds a context to your data.
You can query your data with aggregates, algebraic and logical operators.

## Usage

The engine will interpret the function syntax, validate it and recall the Eval function on you implementation class, recursing all calls.

Aggregate Operators: 
    ag - main aggregation operator
    fid - formula id, usefull to implement as a persisted formula

Algebraic Operators: 
    + - Add 
    * - Mult 
    / - Div

Transform Operators: 
    sc - value on scale1 to scale2

Logical Operators: 
    & - and
    | - or
    < - less then 
    > - bigger then
    in - in between 2 values

## Support

It is currently working with millions of crypto currency tickers and indicators at www.sidegence.com

For support, email sidegence@gmail.com


## Optimizations
1)
All Eval args have default values, so can be queried as "ag(Sum, Quantity, Fr.H1)"  means ''Get sum of quantity for the last hour''

2)
When quering millions of records to aggregate, make sure to use a fast data series access - EF may not do the job - Stored Procs may be more effective.
It is up to your implementation which data series access to use. This lib will only manipulate results, based on the syntax.