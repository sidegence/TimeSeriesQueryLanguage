using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Samples.ClientEvalImplementations;
using TimeSeriesQueryLanguage.Samples.Persistence;

namespace TimeSeriesQueryLanguage.Samples.Mappings
{
    public static class Helper
    {
        public static T Convert<T>(string v) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), v);
        }
        public static Expression<Func<Ticker, decimal>> Map(AggregateColumnsEnum aggCl)
        {
            switch (aggCl)
            {
                case AggregateColumnsEnum.price: return (_) => _.price;
                case AggregateColumnsEnum.qty: return (_) => _.qty;
                default:
                    throw new Exception($"Map({aggCl}) not implemented.");
            }
        }
        public static decimal Map(Ticker? t, AggregateColumnsEnum aggCl)
        {
            if (t == null)
                return 0.0m;

            switch (aggCl)
            {
                case AggregateColumnsEnum.price: return t.price;
                case AggregateColumnsEnum.qty: return t.qty;
                default:
                    throw new Exception($"Map({aggCl}) not implemented.");
            }
        }
    }
}
