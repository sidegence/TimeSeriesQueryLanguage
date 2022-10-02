using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Samples.Persistence;

namespace TimeSeriesQueryLanguage.Samples.Mappings
{
    public static class AggClToTimeSeriesColumnMapping
    {
        //public static Expression<Func<Ticker, decimal>> Map(AggCl aggCl)
        //{
        //    switch (aggCl)
        //    { 
        //        case AggCl.Cl0 : return (_) => _.price;
        //        case AggCl.Cl1: return (_) => _.qty;
        //        default:
        //            throw new Exception($"AggClToTimeSeriesColumnMapping({aggCl}) not implemented.");
        //    }
        //}
        //public static decimal Map(Ticker t, AggCl aggCl)
        //{
        //    switch (aggCl)
        //    {
        //        case AggCl.Cl0: return t.price;
        //        case AggCl.Cl1: return t.qty;
        //        default:
        //            throw new Exception($"AggClToTimeSeriesColumnMapping({aggCl}) not implemented.");
        //    }
        //}
    }
}
