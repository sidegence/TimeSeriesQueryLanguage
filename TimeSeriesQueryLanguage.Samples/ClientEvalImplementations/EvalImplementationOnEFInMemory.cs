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
            AggTimeIntervalEnum aggTsSlideTo = AggTimeIntervalEnum.M0,
            AggTimeIntervalEnum aggTsFrame = AggTimeIntervalEnum.M0,
            int i = 0
        ) where TAggFn : Enum where TAggCl : Enum
        {
            if (aggFn == null || !Enum.IsDefined(typeof(TAggFn), aggFn) || aggCl == null || !Enum.IsDefined(typeof(TAggCl), aggCl))
                throw new ArgumentNullException("Eval<TAggFn, TAggCl> type arguments cannot be null or undefined");

            var tsSlideTo = (await Db.Tickers.FirstAsync()).ts - AggTimeIntervalEnumToTimeSpan.Map(aggTsSlideTo);
            var tsFrameMin = tsSlideTo - AggTimeIntervalEnumToTimeSpan.Map(aggTsFrame);
            var tickers = Db.Tickers.Where(_ => _.ts <= tsSlideTo && _.ts >= tsFrameMin);

            var column = Helper.Convert<AggregateColumnsEnum>(aggCl.ToString());
            var columnFunc = Helper.Map(column);

            if (operationEnum == OperationEnum.Fid)
                return i;

            if (operationEnum == OperationEnum.Tid)
                return i;

            switch (Helper.Convert<AggregateFunctionsEnum>(aggFn.ToString()))
            {
                case AggregateFunctionsEnum.Cnt: return await tickers.CountAsync();
                case AggregateFunctionsEnum.Fst: return Helper.Map(await tickers.FirstOrDefaultAsync(), column);
                case AggregateFunctionsEnum.Lst: return Helper.Map(await tickers.LastOrDefaultAsync(), column);
                case AggregateFunctionsEnum.Min: return await tickers.MinAsync(columnFunc);
                case AggregateFunctionsEnum.Max: return await tickers.MaxAsync(columnFunc);
                case AggregateFunctionsEnum.Avg: return await tickers.AverageAsync(columnFunc);
                case AggregateFunctionsEnum.Dlt:
                    {
                        var fst = Helper.Map(await tickers.FirstAsync(), column);
                        var lst = Helper.Map(await tickers.LastAsync(), column);
                        return fst == 0 ? 0 : (lst - fst) / fst * 100.0m;
                    }
                case AggregateFunctionsEnum.MMP:
                    {
                        var min = await tickers.MinAsync(columnFunc);
                        var max = await tickers.MaxAsync(columnFunc);
                        var lst = Helper.Map(await tickers.LastAsync(), column);
                        return min == max ? 50.0m : (lst - min) * (100 - 0) / (max - min);
                    }
            }

            return 0.0m;
        }
    }
}
