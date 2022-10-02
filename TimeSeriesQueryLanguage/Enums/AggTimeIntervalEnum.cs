using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSeriesQueryLanguage.Enums
{
    public enum AggTimeIntervalEnum
    {
        M0 = 0, M1, M2, M5, M10, M15, M30, M45, H1, H2, H3, H5, H8, H17, H23, D1, D2, D3, D4, D5, D6, D7, D28, m1, m3, m6, m9, Y1, Y2, Y5, C1
    }

    public static class AggTimeIntervalEnumToTimeSpan
    {
        public static TimeSpan Map(AggTimeIntervalEnum aggTs)
        {
            switch (aggTs)
            {
                case AggTimeIntervalEnum.M0: return TimeSpan.Zero;
                case AggTimeIntervalEnum.M1: return TimeSpan.FromMinutes(1);
                case AggTimeIntervalEnum.M2: return TimeSpan.FromMinutes(2);
                case AggTimeIntervalEnum.M5: return TimeSpan.FromMinutes(5);
                case AggTimeIntervalEnum.M10: return TimeSpan.FromMinutes(10);
                case AggTimeIntervalEnum.M15: return TimeSpan.FromMinutes(15);
                case AggTimeIntervalEnum.M30: return TimeSpan.FromMinutes(30);
                case AggTimeIntervalEnum.M45: return TimeSpan.FromMinutes(45);

                case AggTimeIntervalEnum.H1: return TimeSpan.FromHours(1);
                case AggTimeIntervalEnum.H2: return TimeSpan.FromHours(2);
                case AggTimeIntervalEnum.H3: return TimeSpan.FromHours(3);
                case AggTimeIntervalEnum.H5: return TimeSpan.FromHours(5);
                case AggTimeIntervalEnum.H8: return TimeSpan.FromHours(8);
                case AggTimeIntervalEnum.H17: return TimeSpan.FromHours(17);
                case AggTimeIntervalEnum.H23: return TimeSpan.FromHours(23);

                case AggTimeIntervalEnum.D1: return TimeSpan.FromDays(1);
                case AggTimeIntervalEnum.D2: return TimeSpan.FromDays(2);
                case AggTimeIntervalEnum.D3: return TimeSpan.FromDays(3);
                case AggTimeIntervalEnum.D4: return TimeSpan.FromDays(4);
                case AggTimeIntervalEnum.D5: return TimeSpan.FromDays(5);
                case AggTimeIntervalEnum.D6: return TimeSpan.FromDays(6);
                case AggTimeIntervalEnum.D7: return TimeSpan.FromDays(7);

                case AggTimeIntervalEnum.m1: return TimeSpan.FromDays(1 * 30);
                case AggTimeIntervalEnum.m3: return TimeSpan.FromDays(3 * 30);
                case AggTimeIntervalEnum.m6: return TimeSpan.FromDays(6 * 30);
                case AggTimeIntervalEnum.m9: return TimeSpan.FromDays(9 * 30);

                case AggTimeIntervalEnum.Y1: return TimeSpan.FromDays(1 * 365);
                case AggTimeIntervalEnum.Y2: return TimeSpan.FromDays(2 * 365);
                case AggTimeIntervalEnum.Y5: return TimeSpan.FromDays(5 * 365);

                case AggTimeIntervalEnum.C1: return TimeSpan.FromDays(100 * 365);

                default:
                    throw new Exception($"AggTsToTimeSpan({aggTs}) not implemented.");
            }
        }
    }
}
