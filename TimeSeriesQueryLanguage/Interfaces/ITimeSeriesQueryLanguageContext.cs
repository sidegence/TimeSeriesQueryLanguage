using TimeSeriesQueryLanguage.Enums;

namespace TimeSeriesQueryLanguage.Interfaces
{
    public interface ITimeSeriesQueryLanguageContext
    {
        Task<decimal> Eval<TAggFn, TAggCl>(
            OperationEnum operationEnum = OperationEnum.Agg,
            TAggFn? aggFn = default,
            TAggCl? aggCl = default, 
            AggTimeIntervalEnum aggTsFr = AggTimeIntervalEnum.Zero, 
            AggTimeIntervalEnum aggTsTo = AggTimeIntervalEnum.Zero,
            int i = 0
        ) where TAggFn : Enum where TAggCl : Enum;
    }
}
