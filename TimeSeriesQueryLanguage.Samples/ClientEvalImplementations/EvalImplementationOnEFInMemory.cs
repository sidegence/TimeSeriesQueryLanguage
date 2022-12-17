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
            var tsqlp = new TimeSeriesQueryLanguageParser<AggregateFunctionsEnum, AggregateColumnsEnum>().Set(fn)?.Parse();
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

            var to = DateTime.UtcNow - AggTimeIntervalEnumToTimeSpan.Map(aggTsTo);
            var fr = to - AggTimeIntervalEnumToTimeSpan.Map(aggTsFr);
            var tickers = Db.Tickers.Where(_ => _.ts >= fr && _.ts <= to);

            var column = Helper.Convert<AggregateColumnsEnum>(aggCl.ToString());
            var columnFunc = Helper.Map(column);

            if (operationEnum == OperationEnum.Fid)
                return i;

            switch (Helper.Convert<AggregateFunctionsEnum>(aggFn.ToString()))
            {
                case AggregateFunctionsEnum.Cnt: return await tickers.CountAsync();
                case AggregateFunctionsEnum.Fst: return Helper.Map(await tickers.FirstOrDefaultAsync(), column);
                case AggregateFunctionsEnum.Lst: return Helper.Map(await tickers.LastOrDefaultAsync(), column);
                case AggregateFunctionsEnum.Min: return await tickers.MinAsync(columnFunc);
                case AggregateFunctionsEnum.Max: return await tickers.MaxAsync(columnFunc);
                case AggregateFunctionsEnum.Avg: return await tickers.AverageAsync(columnFunc);
                case AggregateFunctionsEnum.Dlt: return (Helper.Map(await tickers.LastOrDefaultAsync(), column) - Helper.Map(await tickers.FirstOrDefaultAsync(), column)) / Helper.Map(await tickers.FirstOrDefaultAsync(), column) * 100m;
            }

            return 0.0m;
        }
    }
}
