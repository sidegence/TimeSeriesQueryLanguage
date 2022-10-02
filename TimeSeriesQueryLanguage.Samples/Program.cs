﻿using Microsoft.EntityFrameworkCore;
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
var eval1 = await i1.Eval<AggegrateFunctions, AggegrateColumns>(AggegrateFunctions.Cnt);
Console.WriteLine("Eval(Cnt) ...: " + eval1);
Console.WriteLine();

var json = File.ReadAllText(@"Persistence\db1.json");
Console.WriteLine("EvalImplementationOnAJsonStore");
var i2 = new EvalImplementationOnAJsonStore(JsonConvert.DeserializeObject<List<Ticker>>(json));
Console.WriteLine("Eval(Cnt) ...: " + await i2.Eval<AggegrateFunctions, AggegrateColumns>(AggegrateFunctions.Cnt));
Console.WriteLine("Eval(Fst, price) ...: " + await i2.Eval(AggegrateFunctions.Fst, AggegrateColumns.price));
Console.WriteLine("Eval(Lst, price) ...: " + await i2.Eval(AggegrateFunctions.Lst, AggegrateColumns.price));
Console.WriteLine("Eval(Fst, qty) ...: " + await i2.Eval(AggegrateFunctions.Fst, AggegrateColumns.qty));
Console.WriteLine("Eval(Lst, qty) ...: " + await i2.Eval(AggegrateFunctions.Lst, AggegrateColumns.qty));
Console.WriteLine();

SampleDbContext? sampleDbContext;
await PopulateDb(60 * 60 * 24);
if (sampleDbContext == null) return;
Console.WriteLine("EvalImplementationOnEFInMemory");
var i3 = new EvalImplementationOnEFInMemory(sampleDbContext);
Console.WriteLine("Eval(AggFn.Cnt) ...: " + await i3.Eval<AggegrateFunctions, AggegrateColumns>(AggegrateFunctions.Cnt));
Console.WriteLine("Eval(Fst, price) ...: " + await i3.Eval(AggegrateFunctions.Fst, AggegrateColumns.price));
Console.WriteLine("Eval(Lst, price) ...: " + await i3.Eval(AggegrateFunctions.Lst, AggegrateColumns.price));
Console.WriteLine("Eval(Avg, price) ...: " + await i3.Eval(AggegrateFunctions.Avg, AggegrateColumns.price));
//Console.WriteLine("[First Price last 5 minutes] => Eval(ag(Fst,Cl0,To.M0,Fr.M5)) ...: " + await i3.Eval("ag(Fst,Cl0,To.M0,Fr.M5)"));
//Console.WriteLine("[Last  Price last 5 minutes] => Eval(ag(Lst,Cl0,To.M0,Fr.M5)) ...: " + await i3.Eval("ag(Lst,Cl0,To.M0,Fr.M5)"));
//Console.WriteLine("[Delta Price last 5 minutes] => Eval(ag(Dlt,Cl0,To.M0,Fr.M5)) ...: " + await i3.Eval("ag(Dlt,Cl0,To.M0,Fr.M5)"));
//Console.WriteLine("[First Price second 5 minutes window] => Eval(ag(Fst,Cl0,To.M5,Fr.M5)) ...: " + await i3.Eval("ag(Fst,Cl0,To.M5,Fr.M5)"));
//Console.WriteLine("[Last  Price second 5 minutes window] => Eval(ag(Lst,Cl0,To.M5,Fr.M5)) ...: " + await i3.Eval("ag(Lst,Cl0,To.M5,Fr.M5)"));
//Console.WriteLine("[Delta Price second 5 minutes window] => Eval(ag(Dlt,Cl0,To.M5,Fr.M5)) ...: " + await i3.Eval("ag(Dlt,Cl0,To.M5,Fr.M5)"));
//Console.WriteLine("[Are last two 5 mins windows V shaped ?] => Eval(&(<(ag(Dlt,Cl0,To.M0,Fr.M5),0),>(ag(Dlt,Cl0,To.M5,Fr.M5),0))) ...: " + await i3.Eval("&(<(ag(Dlt,Cl0,To.M0,Fr.M5),0),>(ag(Dlt,Cl0,To.M5,Fr.M5),0))"));
//Console.WriteLine("[Are last two 5 mins windows M shaped ?] => Eval(&(>(ag(Dlt,Cl0,To.M0,Fr.M5),0),<(ag(Dlt,Cl0,To.M5,Fr.M5),0))) ...: " + await i3.Eval("&(>(ag(Dlt,Cl0,To.M0,Fr.M5),0),<(ag(Dlt,Cl0,To.M5,Fr.M5),0))"));
//Console.WriteLine("[Current Price to MinMax Price position last 05m] => Eval(ag(MMP,Cl0,To.M0,Fr.M5)) ...: " + await i3.Eval("ag(MMP,Cl0,To.M0,Fr.M5)"));
//Console.WriteLine("[Current Price to MinMax Price position last 15m] => Eval(ag(MMP,Cl0,To.M0,Fr.M15)) ...: " + await i3.Eval("ag(MMP,Cl0,To.M0,Fr.M15)"));
//Console.WriteLine("[Current Price to MinMax Price position last 01h] => Eval(ag(MMP,Cl0,To.M0,Fr.H1)) ...: " + await i3.Eval("ag(MMP,Cl0,To.M0,Fr.H1)"));
//Console.WriteLine("[Current Price to MinMax Price position last 24h] => Eval(ag(MMP,Cl0,To.M0,Fr.D1)) ...: " + await i3.Eval("ag(MMP,Cl0,To.M0,Fr.D1)"));
//Console.WriteLine("[fid(1) ...: " + await i3.Eval("fid(1)"));
//Console.WriteLine("[tid(2) ...: " + await i3.Eval("tid(Lst,2,To.M0,Fr.M5)"));

Console.WriteLine();

async Task PopulateDb(int numberOfSeconds)
{
    var services = new ServiceCollection();
    services.AddDbContext<SampleDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"));
    var serviceProvider = services.BuildServiceProvider();
    sampleDbContext = serviceProvider.GetService<SampleDbContext>();
    if (sampleDbContext == null) return;
    var price = 100.0m;
    var rnd = new Random();
    for (int i = 1; i <= numberOfSeconds; i++)
    {
        await sampleDbContext.Tickers.AddAsync(
            new Ticker()
            {
                id = i,
                ts = DateTime.UtcNow.AddSeconds(-(i - 1)),
                price = price,
                qty = 0.1m + (decimal)(rnd.NextDouble()),
                side = rnd.NextDouble() < 0.5 ? "b" : "s"
            });
        price += (rnd.NextDouble() < 0.5 ? 1 : -1) * (decimal)(rnd.NextDouble());
    }
    await sampleDbContext.SaveChangesAsync();
}