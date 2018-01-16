using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Core.Utilities
{
    /// <summary>
    /// 时区辅助类
    /// </summary>
    public class TimeZoneUtility
    {
        /// <summary>
        /// 根据与UTC的时差值，获取时区信息
        /// </summary>
        /// <param name="offsetHour">与UTC的时差值</param>
        /// <returns></returns>
        public static TimeZoneInfo GetTimeZoneByOffsetHours(decimal? offsetHour)
        {
            if (offsetHour.HasValue)
            {
                var timeZone = GetTimeZones().FirstOrDefault(m => m.BaseUtcOffset.TotalHours == (double)offsetHour);
                timeZone = (timeZone ?? TimeZoneInfo.Local);
                return timeZone;
            }

            return TimeZoneInfo.Local;
        }

        /// <summary>
        /// 获取系统的所有时区
        /// </summary>
        /// <returns></returns>
        public static ICollection<TimeZoneInfo> GetTimeZones()
        {
            ICollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
            return timeZones;
        }

        public static TimeZoneInfo FindSystemTimeZoneById(string timeZoneId)
        {
            TimeZoneInfo timeZoneInfo = null;
            if (String.IsNullOrEmpty(timeZoneId))
            {
                timeZoneInfo = TimeZoneInfo.Local;
            }
            else
            {
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }

            return timeZoneInfo;
        }
    }
}
