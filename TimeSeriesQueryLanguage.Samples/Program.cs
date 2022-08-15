using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics;
using TimeSeriesQueryLanguage.Core;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;
using TimeSeriesQueryLanguage.Samples.ClientEvalImplementations;
using TimeSeriesQueryLanguage.Samples.Persistence;

Console.WriteLine("TimeSeries Query Language Samples");
Console.WriteLine();

Console.WriteLine("EvalImplementationThatAlwaysReturns42");
var evalImplementationThatAlwaysReturns42 = new EvalImplementationThatAlwaysReturns42();
var eval1 = await evalImplementationThatAlwaysReturns42.Eval(AggFn.Cnt);
Console.WriteLine("Eval(AggFn.Cnt) ...: " + eval1);
Console.WriteLine();

var json = File.ReadAllText(@"Persistence\db1.json");
Console.WriteLine("EvalImplementationOnAJsonStore");
var evalImplementationOnAJsonStore = new EvalImplementationOnAJsonStore(JsonConvert.DeserializeObject<List<Ticker>>(json));
Console.WriteLine("Eval(AggFn.Cnt) ...: " + await evalImplementationOnAJsonStore.Eval(AggFn.Cnt));
Console.WriteLine("Eval(AggFn.Fst, AggCl.Cl0) ...: " + await evalImplementationOnAJsonStore.Eval(AggFn.Fst, AggCl.Cl0));
Console.WriteLine("Eval(AggFn.Lst, AggCl.Cl0) ...: " + await evalImplementationOnAJsonStore.Eval(AggFn.Lst, AggCl.Cl0));
Console.WriteLine("Eval(AggFn.Avg, AggCl.Cl0) ...: " + await evalImplementationOnAJsonStore.Eval(AggFn.Avg, AggCl.Cl0));
Console.WriteLine();

var services = new ServiceCollection();
services.AddDbContext<SampleDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"));
var serviceProvider = services.BuildServiceProvider();
var sampleDbContext = serviceProvider.GetService<SampleDbContext>();
if (sampleDbContext == null) return;
var price = 100.0m;
var rnd = new Random();
for (int i = 1; i <= 60 * 60 * 24; i++)
{
    await sampleDbContext.Tickers.AddAsync(
        new Ticker() 
        { 
            id = i, ts = DateTime.UtcNow.AddSeconds(-(i-1)), price = price, qty = 0.1m + (decimal)(rnd.NextDouble()), side = rnd.NextDouble() < 0.5 ? "b" : "s"
        });
    price += (rnd.NextDouble() < 0.5 ? 1 : -1 ) * (decimal)(rnd.NextDouble());
}
await sampleDbContext.SaveChangesAsync();

Console.WriteLine("EvalImplementationOnEFInMemory");
var evalImplementationOnEFInMemory = new EvalImplementationOnEFInMemory(sampleDbContext);
Console.WriteLine("Eval(AggFn.Cnt) ...: " + await evalImplementationOnEFInMemory.Eval(AggFn.Cnt));
Console.WriteLine("Eval(AggFn.Fst, AggCl.Cl0) ...: " + await evalImplementationOnEFInMemory.Eval(AggFn.Fst, AggCl.Cl0));
Console.WriteLine("Eval(AggFn.Lst, AggCl.Cl0) ...: " + await evalImplementationOnEFInMemory.Eval(AggFn.Lst, AggCl.Cl0));
Console.WriteLine("Eval(AggFn.Avg, AggCl.Cl0) ...: " + await evalImplementationOnEFInMemory.Eval(AggFn.Avg, AggCl.Cl0));
Console.WriteLine("[first price last 5 minutes] => Eval(ag(Fst,Cl0,To.M0,Fr.M5)) ...: " + await evalImplementationOnEFInMemory.Eval("ag(Fst,Cl0,To.M0,Fr.M5)"));
Console.WriteLine("[last  price last 5 minutes] => Eval(ag(Lst,Cl0,To.M0,Fr.M5)) ...: " + await evalImplementationOnEFInMemory.Eval("ag(Lst,Cl0,To.M0,Fr.M5)"));
Console.WriteLine("[delta price last 5 minutes] => Eval(ag(Dlt,Cl0,To.M0,Fr.M5)) ...: " + await evalImplementationOnEFInMemory.Eval("ag(Dlt,Cl0,To.M0,Fr.M5)"));
Console.WriteLine("[first price second 5 minutes window] => Eval(ag(Fst,Cl0,To.M5,Fr.M5)) ...: " + await evalImplementationOnEFInMemory.Eval("ag(Fst,Cl0,To.M5,Fr.M5)"));
Console.WriteLine("[last  price second 5 minutes window] => Eval(ag(Lst,Cl0,To.M5,Fr.M5)) ...: " + await evalImplementationOnEFInMemory.Eval("ag(Lst,Cl0,To.M5,Fr.M5)"));
Console.WriteLine("[delta price second 5 minutes window] => Eval(ag(Dlt,Cl0,To.M5,Fr.M5)) ...: " + await evalImplementationOnEFInMemory.Eval("ag(Dlt,Cl0,To.M5,Fr.M5)"));
Console.WriteLine("[are last two 5 mins windows V shaped ?] => Eval(&(<(ag(Dlt,Cl0,To.M0,Fr.M5),0),>(ag(Dlt,Cl0,To.M5,Fr.M5),0))) ...: " + await evalImplementationOnEFInMemory.Eval("&(<(ag(Dlt,Cl0,To.M0,Fr.M5),0),>(ag(Dlt,Cl0,To.M5,Fr.M5),0))"));
Console.WriteLine("[are last two 5 mins windows M shaped ?] => Eval(&(>(ag(Dlt,Cl0,To.M0,Fr.M5),0),<(ag(Dlt,Cl0,To.M5,Fr.M5),0))) ...: " + await evalImplementationOnEFInMemory.Eval("&(>(ag(Dlt,Cl0,To.M0,Fr.M5),0),<(ag(Dlt,Cl0,To.M5,Fr.M5),0))"));
Console.WriteLine("[current price to MinMax Price position last 05m] => Eval(ag(MMP,Cl0,To.M0,Fr.M5)) ...: " + await evalImplementationOnEFInMemory.Eval("ag(MMP,Cl0,To.M0,Fr.M5)"));
Console.WriteLine("[current price to MinMax Price position last 15m] => Eval(ag(MMP,Cl0,To.M0,Fr.M15)) ...: " + await evalImplementationOnEFInMemory.Eval("ag(MMP,Cl0,To.M0,Fr.M15)"));
Console.WriteLine("[current price to MinMax Price position last 01h] => Eval(ag(MMP,Cl0,To.M0,Fr.H1)) ...: " + await evalImplementationOnEFInMemory.Eval("ag(MMP,Cl0,To.M0,Fr.H1)"));
Console.WriteLine("[current price to MinMax Price position last 24h] => Eval(ag(MMP,Cl0,To.M0,Fr.D1)) ...: " + await evalImplementationOnEFInMemory.Eval("ag(MMP,Cl0,To.M0,Fr.D1)"));



Console.WriteLine();
