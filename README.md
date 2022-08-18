
# TimeSeries Query Language

This is a library to aggregate values on user defined time series datasets.

## Quick Start

```
public class YOUREvalImplementationOnEFInMemory : ITimeSeriesQueryLanguageContext
{
    readonly YOURDbContext Db;
    public EvalImplementationOnEFInMemory(YOURDbContext db)
    {
        Db = db;
    }
    public async Task<decimal> Eval(string fn)
    {
        var tsqlp = new TimeSeriesQueryLanguageParser().Set(fn)?.Parse();
        return tsqlp == null ? 0.0m : await tsqlp.Eval(this);
    }
    public async Task<decimal> Eval(AggFn aggFn, AggCl aggCl = AggCl.Cl0, AggTs aggTsSlideTo = AggTs.M0, AggTs aggTsFrame = AggTs.D7, int i = 0)
    {
        var tsSlideTo = (await Db.Tickers.FirstAsync()).ts - AggTsToTimeSpanMapping.Map(aggTsSlideTo);
        var tsFrameMin = tsSlideTo - AggTsToTimeSpanMapping.Map(aggTsFrame);
        var tickers = Db.Tickers.Where(_ => _.ts <= tsSlideTo && _.ts >= tsFrameMin);

        switch (aggFn)
        {
            case AggFn.Cnt: return await tickers.CountAsync();
        }
        return 0.0m;
    }
}

// Count all rows in data
var c1 = await evalImplementationOnEFInMemory.Eval(AggFn.Cnt));

// Count rows in data for the last 5 mins
var c2 = await evalImplementationOnEFInMemory.Eval("ag(Cnt,Cl0,To.M0,Fr.M5)");										

// Count rows in data for the last 5 mins, starting 1 hour ago 
var c2 = await evalImplementationOnEFInMemory.Eval("ag(Cnt,Cl0,To.H1,Fr.M5)");										
```

Open the Samples project to see it in action with many more examples.

The Eval function:
This function lives within your class implementation, that also holds a context to your data.
You can query your data with aggregates, algebraic and logical operators.
As the engine doesnt have knowledge of your dataset column names, a mapping will be needed.

## Usage

The engine will interpret the function syntax, validate it and recall the Eval function on you implementation class, recursing all calls.

Aggregate Operators: ag
Algebraic Operators: +, *, /, sc
Logical Operators: &, |, <, >, in

Task<decimal> Eval(AggFn aggFn, AggCl aggCl = AggCl.Cl0, AggTs aggTsSlideTo = AggTs.M0, AggTs aggTsFrame = AggTs.M0, int i = 0)
AggFn : Cnt, TxS, TxM, TxH, Fst, Snd, Pen, Lst, Min, Max, Avg, Sum, Dlt, MMP, FId, StD
Used for aggregation data on your data implementation class

AggCl : Cl0, Cl1, Cl2, Cl3, Cl4, Cl5, Cl6, Cl7, Cl8, Cl9,
Used for mapping to you data column fields

AggTs : M0 = 0, M1, M2, M5, M10, M15, M30, M45, H1, H2, H3, H5, H8, H17, H23, D1, D2, D3, D4, D5, D6, D7
Used for windowing the time series. aggTsSlideTo argument says how far back you want to go on your data. aggTsFrame argument defines the window time size.


## Support

For support, email sidegence@gmail.com


## Optimizations

Mappings are still needed for translating AggCl internal columns to your own dataset. 
