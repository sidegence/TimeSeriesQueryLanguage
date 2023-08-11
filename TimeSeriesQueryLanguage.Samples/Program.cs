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
var i1 = new EvalImplementationThatAlwaysReturns42();
var eval1 = await i1.Eval<AggregateFunctionsEnum, AggregateColumnsEnum>(aggFn: AggregateFunctionsEnum.Cnt);
Console.WriteLine("Eval(Cnt) ...: " + eval1);
Console.WriteLine();

var json = File.ReadAllText(@"Persistence\db1.json");
Console.WriteLine("EvalImplementationOnAJsonStore");
var i2 = new EvalImplementationOnAJsonStore(JsonConvert.DeserializeObject<List<Ticker>>(json));
Console.WriteLine("Eval(Cnt) ...: " + await i2.Eval<AggregateFunctionsEnum, AggregateColumnsEnum>(aggFn: AggregateFunctionsEnum.Cnt));
Console.WriteLine("Eval(Fst, price) ...: " + await i2.Eval(aggFn: AggregateFunctionsEnum.Fst, aggCl: AggregateColumnsEnum.price));
Console.WriteLine("Eval(Lst, price) ...: " + await i2.Eval(aggFn: AggregateFunctionsEnum.Lst, aggCl: AggregateColumnsEnum.price));
Console.WriteLine("Eval(Avg, price) ...: " + await i2.Eval(aggFn: AggregateFunctionsEnum.Avg, aggCl: AggregateColumnsEnum.price));
Console.WriteLine("Eval(Fst, qty) ...: " + await i2.Eval(aggFn: AggregateFunctionsEnum.Fst, aggCl: AggregateColumnsEnum.qty));
Console.WriteLine("Eval(Lst, qty) ...: " + await i2.Eval(aggFn: AggregateFunctionsEnum.Lst, aggCl: AggregateColumnsEnum.qty));
Console.WriteLine("Eval(Avg, qty) ...: " + await i2.Eval(aggFn: AggregateFunctionsEnum.Avg, aggCl: AggregateColumnsEnum.qty));
Console.WriteLine();

SampleDbContext? sampleDbContext = null!;
await PopulateDb(10);
if (sampleDbContext == null) return;
PrintDb();
Console.WriteLine("EvalImplementationOnEFInMemory");
var i3 = new EvalImplementationOnEFInMemory(sampleDbContext);
Console.WriteLine("Eval(AggFn.Cnt)  All ...: " + await i3.Eval<AggregateFunctionsEnum, AggregateColumnsEnum>(aggFn: AggregateFunctionsEnum.Cnt, aggTsFr: AggTimeIntervalEnum.C1));
Console.WriteLine("Eval(Fst, price) All ...: " + await i3.Eval(aggFn: AggregateFunctionsEnum.Fst, aggCl: AggregateColumnsEnum.price, aggTsFr: AggTimeIntervalEnum.C1));
Console.WriteLine("Eval(Lst, price) All ...: " + await i3.Eval(aggFn: AggregateFunctionsEnum.Lst, aggCl: AggregateColumnsEnum.price, aggTsFr: AggTimeIntervalEnum.C1));
Console.WriteLine("Eval(Avg, price) All ...: " + await i3.Eval(aggFn: AggregateFunctionsEnum.Avg, aggCl: AggregateColumnsEnum.price, aggTsFr: AggTimeIntervalEnum.C1));
Console.WriteLine("Eval(Min, price) All ...: " + await i3.Eval(aggFn: AggregateFunctionsEnum.Min, aggCl: AggregateColumnsEnum.price, aggTsFr: AggTimeIntervalEnum.C1));
Console.WriteLine("Eval(Max, price) All ...: " + await i3.Eval(aggFn: AggregateFunctionsEnum.Max, aggCl: AggregateColumnsEnum.price, aggTsFr: AggTimeIntervalEnum.C1));
Console.WriteLine("Eval(Dlt, price) All ...: " + await i3.Eval(aggFn: AggregateFunctionsEnum.Dlt, aggCl: AggregateColumnsEnum.price, aggTsFr: AggTimeIntervalEnum.C1));

Console.WriteLine("Eval(AggFn.Cnt)  5min...: " + await i3.Eval<AggregateFunctionsEnum, AggregateColumnsEnum>(aggFn: AggregateFunctionsEnum.Cnt, aggTsFr: AggTimeIntervalEnum.M5));
Console.WriteLine("Eval(Fst, price) 5min...: " + await i3.Eval(aggFn: AggregateFunctionsEnum.Fst, aggCl: AggregateColumnsEnum.price, aggTsFr: AggTimeIntervalEnum.M5));
Console.WriteLine("Eval(Lst, price) 5min...: " + await i3.Eval(aggFn: AggregateFunctionsEnum.Lst, aggCl: AggregateColumnsEnum.price, aggTsFr: AggTimeIntervalEnum.M5));
Console.WriteLine("Eval(Avg, price) 5min...: " + await i3.Eval(aggFn: AggregateFunctionsEnum.Avg, aggCl: AggregateColumnsEnum.price, aggTsFr: AggTimeIntervalEnum.M5));
Console.WriteLine("Eval(Min, price) 5min...: " + await i3.Eval(aggFn: AggregateFunctionsEnum.Min, aggCl: AggregateColumnsEnum.price, aggTsFr: AggTimeIntervalEnum.M5));
Console.WriteLine("Eval(Max, price) 5min...: " + await i3.Eval(aggFn: AggregateFunctionsEnum.Max, aggCl: AggregateColumnsEnum.price, aggTsFr: AggTimeIntervalEnum.M5));
Console.WriteLine("Eval(Dlt, price) 5min...: " + await i3.Eval(aggFn: AggregateFunctionsEnum.Dlt, aggCl: AggregateColumnsEnum.price, aggTsFr: AggTimeIntervalEnum.M5));

Console.WriteLine("Dlt 5min > Dlt All ...: " + await i3.Eval(">(ag(Dlt,price,Fr.M5,To.Zero),ag(Dlt,price,Fr.C1,To.Zero))"));
Console.WriteLine("Dlt 5min < Dlt All ...: " + await i3.Eval("<(ag(Dlt,price,Fr.M5,To.Zero),ag(Dlt,price,Fr.C1,To.Zero))"));

Console.WriteLine();

async Task PopulateDb(int n)
{
    Console.WriteLine($"Populating Db ...({n})");

    var services = new ServiceCollection();
    services.AddDbContext<SampleDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"));
    var serviceProvider = services.BuildServiceProvider();
    sampleDbContext = serviceProvider.GetService<SampleDbContext>();
    if (sampleDbContext == null) return;
    var price = 100.0m;
    var rnd = new Random();
    for (int i = 1; i <= n; i++)
    {
        await sampleDbContext.Tickers.AddAsync(
            new Ticker()
            {
                id = i,
                ts = DateTime.UtcNow.AddMinutes(-(n - i)),
                price = price,
                qty = i, 
                side = rnd.NextDouble() < 0.5 ? "b" : "s"
            });
        price += 1;
    }
    await sampleDbContext.SaveChangesAsync();
}

void PrintDb()
{
    Console.WriteLine(sampleDbContext.Tickers.First().Title());
    Console.WriteLine($"------------------------------------------------------------------------------------");
    foreach (var ticker in sampleDbContext.Tickers)
    {
        Console.WriteLine(ticker);
    }
    Console.WriteLine($"------------------------------------------------------------------------------------");
}