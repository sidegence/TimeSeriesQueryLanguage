using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;

namespace TimeSeriesQueryLanguage.Samples.Mappings
{
    public static class AggTsToTimeSpanMapping
    {
        public static TimeSpan Map(AggTs aggTs)
        {
            switch (aggTs)
            {
                case AggTs.M0: return TimeSpan.Zero;
                case AggTs.M1: return TimeSpan.FromMinutes(1);
                case AggTs.M2: return TimeSpan.FromMinutes(2);
                case AggTs.M5: return TimeSpan.FromMinutes(5);
                case AggTs.M10: return TimeSpan.FromMinutes(10);
                case AggTs.M15: return TimeSpan.FromMinutes(15);
                case AggTs.M30: return TimeSpan.FromMinutes(30);
                case AggTs.M45: return TimeSpan.FromMinutes(45);

                case AggTs.H1: return TimeSpan.FromHours(1);
                case AggTs.H2: return TimeSpan.FromHours(2);
                case AggTs.H3: return TimeSpan.FromHours(3);
                case AggTs.H5: return TimeSpan.FromHours(5);
                case AggTs.H8: return TimeSpan.FromHours(8);
                case AggTs.H17: return TimeSpan.FromHours(17);
                case AggTs.H23: return TimeSpan.FromHours(23);

                case AggTs.D1: return TimeSpan.FromDays(1);
                case AggTs.D2: return TimeSpan.FromDays(2);
                case AggTs.D3: return TimeSpan.FromDays(3);
                case AggTs.D4: return TimeSpan.FromDays(4);
                case AggTs.D5: return TimeSpan.FromDays(5);
                case AggTs.D6: return TimeSpan.FromDays(6);
                case AggTs.D7: return TimeSpan.FromDays(7);

                default:
                    throw new Exception($"AggTsToTimeSpan({aggTs}) not implemented.");
            }
        }
    }
}
