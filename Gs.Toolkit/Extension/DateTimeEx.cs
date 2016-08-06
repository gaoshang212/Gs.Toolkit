using System;

namespace Gs.Toolkit.Extension
{
    public class DateTimeEx
    {
        public static long GetTimestamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds;
        }

        public static DateTime GetDateTime(long p_timestamp)
        {
            var ts = TimeSpan.FromMilliseconds(p_timestamp);

            return new DateTime(1970, 1, 1, 0, 0, 0, 0).Add(ts);
        }
    }
}