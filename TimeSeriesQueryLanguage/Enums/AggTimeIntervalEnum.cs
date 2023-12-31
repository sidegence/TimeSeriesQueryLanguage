namespace TimeSeriesQueryLanguage.Enums
{
    public enum AggTimeIntervalEnum
    {
        Zero = 0, 
            M1, M2, M5, M10, M15, M30, M45, M59,
            H1, H2, H3, H4, H5, H6, H7, H8, H9, H10, H11, H12, H13, H14, H15, H16, H17, H18, H19, H20, H21, H22, H23, 
            D1, D2, D3, D4, D5, D6, D7, D14, D30,
            m1, m3, m6, m9, 
            Y1, Y2, Y5, 
            C1
    }

    public static class AggTimeIntervalEnumToTimeSpan
    {
        public static TimeSpan Map(AggTimeIntervalEnum aggTs)
        {
            switch (aggTs)
            {
                case AggTimeIntervalEnum.Zero: return TimeSpan.Zero;
                case AggTimeIntervalEnum.M1: return TimeSpan.FromMinutes(1);
                case AggTimeIntervalEnum.M2: return TimeSpan.FromMinutes(2);
                case AggTimeIntervalEnum.M5: return TimeSpan.FromMinutes(5);
                case AggTimeIntervalEnum.M10: return TimeSpan.FromMinutes(10);
                case AggTimeIntervalEnum.M15: return TimeSpan.FromMinutes(15);
                case AggTimeIntervalEnum.M30: return TimeSpan.FromMinutes(30);
                case AggTimeIntervalEnum.M45: return TimeSpan.FromMinutes(45);
                case AggTimeIntervalEnum.M59: return TimeSpan.FromMinutes(59);

                case AggTimeIntervalEnum.H1: return TimeSpan.FromHours(1);
                case AggTimeIntervalEnum.H2: return TimeSpan.FromHours(2);
                case AggTimeIntervalEnum.H3: return TimeSpan.FromHours(3);
                case AggTimeIntervalEnum.H4: return TimeSpan.FromHours(4);
                case AggTimeIntervalEnum.H5: return TimeSpan.FromHours(5);
                case AggTimeIntervalEnum.H6: return TimeSpan.FromHours(6);
                case AggTimeIntervalEnum.H7: return TimeSpan.FromHours(7);
                case AggTimeIntervalEnum.H8: return TimeSpan.FromHours(8);
                case AggTimeIntervalEnum.H9: return TimeSpan.FromHours(9);
                case AggTimeIntervalEnum.H10: return TimeSpan.FromHours(10);
                case AggTimeIntervalEnum.H11: return TimeSpan.FromHours(11);
                case AggTimeIntervalEnum.H12: return TimeSpan.FromHours(12);
                case AggTimeIntervalEnum.H13: return TimeSpan.FromHours(13);
                case AggTimeIntervalEnum.H14: return TimeSpan.FromHours(14);
                case AggTimeIntervalEnum.H15: return TimeSpan.FromHours(15);
                case AggTimeIntervalEnum.H16: return TimeSpan.FromHours(16);
                case AggTimeIntervalEnum.H17: return TimeSpan.FromHours(17);
                case AggTimeIntervalEnum.H18: return TimeSpan.FromHours(18);
                case AggTimeIntervalEnum.H19: return TimeSpan.FromHours(19);
                case AggTimeIntervalEnum.H20: return TimeSpan.FromHours(20);
                case AggTimeIntervalEnum.H21: return TimeSpan.FromHours(21);
                case AggTimeIntervalEnum.H22: return TimeSpan.FromHours(22);
                case AggTimeIntervalEnum.H23: return TimeSpan.FromHours(23);

                case AggTimeIntervalEnum.D1: return TimeSpan.FromDays(1);
                case AggTimeIntervalEnum.D2: return TimeSpan.FromDays(2);
                case AggTimeIntervalEnum.D3: return TimeSpan.FromDays(3);
                case AggTimeIntervalEnum.D4: return TimeSpan.FromDays(4);
                case AggTimeIntervalEnum.D5: return TimeSpan.FromDays(5);
                case AggTimeIntervalEnum.D6: return TimeSpan.FromDays(6);
                case AggTimeIntervalEnum.D7: return TimeSpan.FromDays(7);
                case AggTimeIntervalEnum.D14: return TimeSpan.FromDays(14);
                case AggTimeIntervalEnum.D30: return TimeSpan.FromDays(30);

                case AggTimeIntervalEnum.m1: return TimeSpan.FromDays(1 * 30);
                case AggTimeIntervalEnum.m3: return TimeSpan.FromDays(3 * 30);
                case AggTimeIntervalEnum.m6: return TimeSpan.FromDays(6 * 30);
                case AggTimeIntervalEnum.m9: return TimeSpan.FromDays(9 * 30);

                case AggTimeIntervalEnum.Y1: return TimeSpan.FromDays(1 * 365);
                case AggTimeIntervalEnum.Y2: return TimeSpan.FromDays(2 * 365);
                case AggTimeIntervalEnum.Y5: return TimeSpan.FromDays(5 * 365);

                case AggTimeIntervalEnum.C1: return TimeSpan.FromDays(100 * 365);

                default:
                    throw new Exception($"AggTimeIntervalEnum({aggTs}) not implemented.");
            }
        }
    }
}
