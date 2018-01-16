using System;
using System.Collections.Generic;
using System.Linq;
using Steven.Core.Utilities;

namespace Steven.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToDisplayDateTime(this DateTime dtime)
        {
            return dtime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToDisplayDateTime2(this DateTime dtime)
        {
            return dtime.ToString("yyyy年MM月dd HH:mm");
        }

        public static string ToDisplayDateTime(this DateTime? dtime)
        {
            return !dtime.HasValue ? "" : dtime.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToDisplayDateTimeMinute(this DateTime dtime)
        {
            return dtime.ToString("yyyy-MM-dd HH:mm");
        }

        public static string ToDisplayWithSprit(this DateTime dtime)
        {
            return dtime.ToString("yyyy/MM/dd HH:mm");
        }

        private readonly static IFormatProvider CultureEnUS = new System.Globalization.CultureInfo("en-US", true);

        public static string ToEnUSTimeFormat(this DateTime dtime)
        {
            return dtime.ToString("hh:mmt\\M,MMM dd,yyyy", CultureEnUS);
        }

        public static string ToEnUSDateFormat(this DateTime dtime)
        {
            return dtime.ToString("MMM dd,yyyy", CultureEnUS);
        }

        public static string ToDisplayDate(this DateTime dtime)
        {
            return dtime.ToString("yyyy-MM-dd");
        }
        public static string ToDisplayDate(this DateTime? dtime)
        {
            return !dtime.HasValue ? "" : dtime.Value.ToString("yyyy-MM-dd");
        }

        public static string ToFormatMd(this DateTime dtime)
        {
            return dtime.ToString("MM-dd");
        }

        #region // 时区之间的转换

        /// <summary>
        /// 将dateTime（包含时区信息）转换为指定的时区
        /// </summary>
        /// <param name="dateTime">包含时区信息的时间</param>
        /// <param name="timeZoneId">指定的时区</param>
        /// <returns></returns>
        public static DateTime ToZoneTime(this DateTime dateTime, string timeZoneId = null)
        {
            return ToZoneTime(dateTime, TimeZoneInfo.Local.Id, timeZoneId);
        }

        /// <summary>
        /// 将指定时区转换为北京时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="timeZoneId">指定的时区</param>
        /// <returns></returns>
        public static DateTime ToBeijingZoneTime(this DateTime dateTime, string timeZoneId)
        {
            return dateTime.ToZoneTime(timeZoneId, "China Standard Time");
        }

        /// <summary>
        /// 美国东部时间（EST）转换为北京时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime EstToBjZoneTime(this DateTime dateTime)
        {
            return dateTime.ToZoneTime("Eastern Standard Time", "China Standard Time");
        }

        /// <summary>
        /// 将北京时间转换为美国东部时间
        /// </summary>
        /// <param name="dateTime">北京时间（东八区）</param>
        /// <returns></returns>
        public static DateTime ToEstTime(this DateTime dateTime)
        {
            return dateTime.ToZoneTime("China Standard Time", "Eastern Standard Time");
        }

        /// <summary>
        /// 在任意两个时区之间转换
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="srcTimeZoneId">指定时区的时区</param>
        /// <param name="descTimeZoneId">目标时区的时区</param>
        /// <returns></returns>
        public static DateTime ToZoneTime(this DateTime dateTime, string srcTimeZoneId, string descTimeZoneId)
        {
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            TimeZoneInfo srcTimeZone = TimeZoneUtility.FindSystemTimeZoneById(srcTimeZoneId);
            TimeZoneInfo descTimeZone = TimeZoneUtility.FindSystemTimeZoneById(descTimeZoneId);

            return TimeZoneInfo.ConvertTime(dateTime, srcTimeZone, descTimeZone);
        }
        #endregion

        public static string DateFormatToString(this DateTime dt)
        {
            TimeSpan span = (DateTime.Now - dt).Duration();

            if (span.TotalDays > 365)
            {
                return (int)Math.Floor(span.TotalDays / 365) + "年前";
            }
            else if (span.TotalDays > 30)
            {
                return (int)Math.Floor((span.TotalDays / 30)) + "月前";
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return "刚刚";
            }
            else
            {
                return "刚刚";
            }
        }
    }
}