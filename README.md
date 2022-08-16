TimeSeries Query Language

This is a library to aggregate values on user defined time series datasets.

QuickStart:

var evalImplementationOnEFInMemory = new EvalImplementationOnEFInMemory(yourDbContext);

// Count all rows in data
var a = await evalImplementationOnEFInMemory.Eval(AggFn.Cnt); 														

// Get the first Cl0(your mapping will give the value: price, qty, etc) value for the last 5 minutes
var b = await evalImplementationOnEFInMemory.Eval("ag(Fst,Cl0,To.M0,Fr.M5)");										

// Get the first Cl0 value for the second 5 minutes window
var c = await evalImplementationOnEFInMemory.Eval("ag(Fst,Cl0,To.M5,Fr.M5)");										

// Asking if, the Delta on Cl0 is less than 0 (decreasing) on the last 5 minutes window and is bigger than 0 (increasing) on the second 5 minutes window
var d = await evalImplementationOnEFInMemory.Eval("&(<(ag(Dlt,Cl0,To.M0,Fr.M5),0),>(ag(Dlt,Cl0,To.M5,Fr.M5),0))");	

// Getting the current Cl0 value position, within the Min Max scale for the last 5 minutes
var e = await evalImplementationOnEFInMemory.Eval("ag(MMP,Cl0,To.M0,Fr.M5)");										

Open the Samples project to see it in action with many more examples.

The Eval function:
This function lives within your class implementation that also holds a context to your data.
You can query your data with aggregates, algebraic and logical operators.
As the engine doesnt have knowledge of your dataset column names, a mapping will be needed.

Usage:
To set up the language parser you need to declare it:
var tsqlp = new TimeSeriesQueryLanguageParser().Set("<(0,1)")?.Parse();
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
