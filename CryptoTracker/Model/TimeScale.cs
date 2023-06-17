using System;
using System.Collections.Generic;

namespace CryptoTracker.Model
{
    public enum Scale
    {
        H1,
        H6,
        D1,
        D7,
        M1,
        M6,
        M12,
        Max
    }
    public static class TimeScale
    {
        private static readonly Dictionary<Scale, DateTime> TimeByScale = new Dictionary<Scale, DateTime>()
        {
            { Scale.H1, DateTime.UtcNow.AddHours(-1) },
            { Scale.H6 , DateTime.UtcNow.AddHours(-6) },

            { Scale.D1 , DateTime.UtcNow.AddDays(-1) },
            { Scale.D7 , DateTime.UtcNow.AddDays(-7) },

            { Scale.M1 , DateTime.UtcNow.AddMonths(-1) },
            { Scale.M6 , DateTime.UtcNow.AddMonths(-6) },
            { Scale.M12 , DateTime.UtcNow.AddMonths(-12) },

            { Scale.Max , DateTime.UtcNow }
        };
        public static DateTime GetPeriod(Scale scale)
        {
            if (TimeByScale.TryGetValue(scale, out DateTime value))
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException("Key not found");
            }
        }
    }
}
