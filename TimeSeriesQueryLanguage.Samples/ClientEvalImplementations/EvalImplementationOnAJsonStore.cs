using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        enum AggegrateColumns { price = 0, qty, side }

        readonly IQueryable<Ticker> Tickers;
        public EvalImplementationOnAJsonStore(List<Ticker> tickers)
        {
            Tickers = tickers.AsQueryable();
        }

        public async Task<decimal> Eval<AggegrateFunctions, T>(
            AggegrateFunctions? aggFn,
            T? aggCl = default,
            AggTs aggTsSlideTo = AggTs.M0,
            AggTs aggTsFrame = AggTs.M0,
            AggegrateFunctions? aggFn2 = default,
            int i = 0
        ) where AggegrateFunctions : Enum where T : Enum
        {
            if (aggFn == null || aggCl == null)
                return 0.0m;

            var tsSlideTo = Tickers.Last().ts - AggTsToTimeSpanMapping.Map(aggTsSlideTo);
            var tsFrameMin = tsSlideTo - AggTsToTimeSpanMapping.Map(aggTsFrame);
            var tickers = Tickers.Where(_ => _.ts <= tsSlideTo && _.ts >= tsFrameMin);

            var x = (T) Enum.Parse(typeof(T), aggCl.ToString());

            switch (aggFn.ToString())
            {
                case "Cnt": return await Task.Run(() => tickers.Count());
                //case "Fst": return await Task.Run(() => AggClToTimeSeriesColumnMapping.Map(tickers.First(), aggCl));
                //case "Lst": return await Task.Run(() => AggClToTimeSeriesColumnMapping.Map(tickers.Last(), aggCl));
                case "Avg": return await Task.Run(() => tickers.Average(_ => Map((AggegrateColumns)(int)x)));
            }

            return 0.0m;
        }
        static Expression<Func<Ticker, decimal>> Map(AggegrateColumns aggegrateColumns)
        {
            switch (aggegrateColumns)
            {
                case AggegrateColumns.price: return (_) => _.price;
                case AggegrateColumns.qty: return (_) => _.qty;
                default:
                    throw new Exception($"Map({aggegrateColumns}) not implemented.");
            }
        }
    }
}
