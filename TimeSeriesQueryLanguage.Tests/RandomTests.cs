using TimeSeriesQueryLanguage.Core;
using TimeSeriesQueryLanguage.Samples.ClientEvalImplementations;

namespace TimeSeriesQueryLanguage.Tests
{
    public class RandomTests
    {
        [Test]
        public void When_using_RandomizeText_should_always_give_a_valid_syntax()
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();

            Parallel.ForEach(Enumerable.Range(1, 1000), _ =>
            {
                var expression = new TimeSeriesQueryLanguageParser<AggregateFunctionsEnum, AggregateColumnsEnum>().Random(5);
                Assert.NotNull(expression);

                var parsing = expression.Parse();
                Assert.NotNull(parsing);

                Assert.DoesNotThrowAsync(async () => await parsing.Eval(clientEvalImplementation));
            });
        }

        public enum AggregateColumnsTicEnum { PRC, CNTB, QTYB, CNTS, QTYS }
        public enum AggregateColumnsIndEnum { PRC, SMA20, EMA12, EMA26, RSI14, MACD, BUB20, BLB20, CMF }
        public enum AggregateFunctionEnum { Cnt, Fst, Snd, Pen, Lst, Min, Max, Avg, Sum, Dlt, MMP, StD, Var }

        [Test]
        public void NOT_TO_COMMIT()
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();
            var size = 25; var start = 1000;
            for (int i = 0; i < size; i++)
            {
                Exe<AggregateColumnsTicEnum>(1 * start + i, "Tic", true, 0);
            }
            for (int i = 0; i < size; i++)
            {
                Exe<AggregateColumnsTicEnum>(2 * start + i, "Tic", false, 0);
            }
            for (int i = 0; i < size; i++)
            {
                Exe<AggregateColumnsIndEnum>(3 * start + i, "Ind", true, 1);
            }
            for (int i = 0; i < size; i++)
            {
                Exe<AggregateColumnsIndEnum>(4 * start + i, "Ind", false, 1);
            }
        }

        private void Exe<T>(int index, string optionEnum, bool objective, int timeseries) where T : Enum
        {
            var clientEvalImplementation = new EvalImplementationThatAlwaysReturns42();

            var expression = new TimeSeriesQueryLanguageParser<AggregateFunctionEnum, T>().Random(3);
            Assert.NotNull(expression);

            var parsing = expression.Parse();
            Assert.NotNull(parsing);
            Assert.DoesNotThrowAsync(async () => await parsing.Eval(clientEvalImplementation));

            var insert = $"insert into [Formulas] ([Id],[Name],[Value],[TimeSeriesType],[Cr],[Eval],[Objective],[Fitness],[Learns],[Observations],[Up])";
            var dttime = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var values = $"{index},'ML.{optionEnum}.{(objective?"Up":"Dn")}.0','{expression.Get()}',{timeseries},'{dttime}',0,{(objective?1:0)},0,1,0,'{dttime}'";
            Console.WriteLine($"{insert} values ({values})");
        }
    }
}