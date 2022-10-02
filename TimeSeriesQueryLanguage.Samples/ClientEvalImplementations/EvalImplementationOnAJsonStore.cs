using Microsoft.EntityFrameworkCore;
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
using static Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;

namespace TimeSeriesQueryLanguage.Samples.ClientEvalImplementations
{
    public class EvalImplementationOnAJsonStore : ITimeSeriesQueryLanguageContext
    {
        readonly IQueryable<Ticker> Tickers;
        public EvalImplementationOnAJsonStore(List<Ticker> tickers)
        {
            Tickers = tickers.AsQueryable();
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
                throw new ArgumentNullException("Eval<TAggFn, TAggCl> type args cannot be null");

            var tickers = Tickers;

            var column = Helper.Convert<AggregateColumnsEnum>(aggCl.ToString());
            var columnFunc = Helper.Map(column);

            switch (Helper.Convert<AggregateFunctionsEnum>(aggFn.ToString()))
            {
                case AggregateFunctionsEnum.Cnt: return tickers.Count();
                case AggregateFunctionsEnum.Fst: return Helper.Map(tickers.FirstOrDefault(), column);
                case AggregateFunctionsEnum.Lst: return Helper.Map(tickers.LastOrDefault(), column);
                case AggregateFunctionsEnum.Avg: return tickers.Average(columnFunc);
            }

            return await Task.FromResult(0.0m);
        }
    }
}
