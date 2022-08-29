using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Interfaces;
using TimeSeriesQueryLanguage.Samples.Mappings;
using TimeSeriesQueryLanguage.Samples.Persistence;

namespace TimeSeriesQueryLanguage.Samples.ClientEvalImplementations
{
    public class EvalImplementationOnAJsonStore : ITimeSeriesQueryLanguageContext
    {
        readonly IQueryable<Ticker> Tickers;
        public EvalImplementationOnAJsonStore(List<Ticker> tickers)
        {
            Tickers = tickers.AsQueryable();
        }

        public async Task<decimal> Eval(AggFn aggFn, AggCl aggCl = AggCl.Cl0, AggTs aggTsSlideTo = AggTs.M0, AggTs aggTsFrame = AggTs.D7, AggFn aggFn2 = AggFn.Cnt, int i = 0)
        {
            var tsSlideTo = Tickers.Last().ts - AggTsToTimeSpanMapping.Map(aggTsSlideTo);
            var tsFrameMin = tsSlideTo - AggTsToTimeSpanMapping.Map(aggTsFrame);
            var tickers = Tickers.Where(_ => _.ts <= tsSlideTo && _.ts >= tsFrameMin);

            switch (aggFn)
            {
                case AggFn.Cnt: return await Task.Run(() => tickers.Count());
                case AggFn.Fst: return await Task.Run(() => AggClToTimeSeriesColumnMapping.Map(tickers.First(), aggCl));
                case AggFn.Lst: return await Task.Run(() => AggClToTimeSeriesColumnMapping.Map(tickers.Last(), aggCl));
                case AggFn.Avg: return await Task.Run(() => tickers.Average(AggClToTimeSeriesColumnMapping.Map(aggCl)));
            }

            return 0.0m;
        }
    }
}
