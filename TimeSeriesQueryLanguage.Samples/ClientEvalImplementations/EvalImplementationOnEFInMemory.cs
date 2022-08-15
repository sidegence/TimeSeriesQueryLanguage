using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Core;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;
using TimeSeriesQueryLanguage.Samples.Mappings;
using TimeSeriesQueryLanguage.Samples.Persistence;

namespace TimeSeriesQueryLanguage.Samples.ClientEvalImplementations
{
    public class EvalImplementationOnEFInMemory : ITimeSeriesQueryLanguageContext
    {
        readonly SampleDbContext Db;
        public EvalImplementationOnEFInMemory(SampleDbContext sampleDbContext)
        {
            Db = sampleDbContext;
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
                case AggFn.Fst: return await Task.FromResult(AggClToTimeSeriesColumnMapping.Map(await tickers.FirstAsync(), aggCl));
                case AggFn.Lst: return await Task.FromResult(AggClToTimeSeriesColumnMapping.Map(await tickers.LastAsync(), aggCl));
                case AggFn.Min: return await tickers.MinAsync(AggClToTimeSeriesColumnMapping.Map(aggCl));
                case AggFn.Max: return await tickers.MaxAsync(AggClToTimeSeriesColumnMapping.Map(aggCl));
                case AggFn.Avg: return await tickers.AverageAsync(AggClToTimeSeriesColumnMapping.Map(aggCl));
                case AggFn.Dlt:
                    {
                        var fst = await Task.FromResult(AggClToTimeSeriesColumnMapping.Map(await tickers.FirstAsync(), aggCl));
                        var lst  = await Task.FromResult(AggClToTimeSeriesColumnMapping.Map(await tickers.LastAsync(), aggCl));
                        return fst == 0 ? 0 : (lst - fst) / fst * 100.0m;
                    }
                case AggFn.MMP:
                    {
                        var min = await tickers.MinAsync(AggClToTimeSeriesColumnMapping.Map(aggCl));
                        var max = await tickers.MaxAsync(AggClToTimeSeriesColumnMapping.Map(aggCl));
                        var lst = await Task.FromResult(AggClToTimeSeriesColumnMapping.Map(await tickers.LastAsync(), aggCl));
                        return min == max ? 50.0m : (lst - min) * (100 - 0) / (max - min);
                    }
            }

            return 0.0m;
        }
    }
}
