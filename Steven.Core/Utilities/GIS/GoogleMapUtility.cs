using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Core.Utilities.GIS
{
    public class GoogleMapUtility
    {
        public static double OneOverPi = 1.0 / Math.PI;
        public static double PiOverTwo = Math.PI * 0.5;
        public static double PiOverThree = Math.PI / 3.0;
        public static double PiOverFour = Math.PI / 4.0;
        public static double PiOverSix = Math.PI / 6.0;
        public static double ThreePiOver2 = (3.0 * Math.PI) * 0.5;
        public static double TwoPi = 2.0 * Math.PI;
        public static double OneOverTwoPi = 1.0 / (2.0 * Math.PI);

        /// <summary>
        /// 地球半径
        /// WGS84 : 6378137.0m
        /// </summary>
        public const double EarthRadius_WGS84 = 6378137.0d;

        /// <summary>
        /// 地球半径
        /// WGS84 : 6378137.0m
        /// Xian80 : 6378140.0m
        /// Beijing54 ：6378245.0m
        /// </summary>
        public double EarthRadius { get; set; }

        /// <summary>
        /// 每度（1°）的弧长
        /// </summary>
        public const double RadiansPerDegree = Math.PI / 180.0;

        /// <summary>
        /// 计算两点间的距离（米）
        /// </summary>
        /// <param name="latA"></param>
        /// <param name="lngA"></param>
        /// <param name="latB"></param>
        /// <param name="lngB"></param>
        /// <returns>两点间的距离（米）</returns>
        public static int GetDistance(decimal latA, decimal lngA, decimal latB, decimal lngB)
        {
            // 计算两点经度之差
            double distanceLng = (double)(lngB - lngA) * RadiansPerDegree;

            // 计算两点纬度之差
            double radLatA = (double)latA * RadiansPerDegree;
            double radLatB = (double)latB * RadiansPerDegree;
            double distanceLat = radLatA - radLatB;

            // 根据勾股定理，计算两点间的距离
            double distance = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(distanceLat / 2), 2) +
                Math.Cos(radLatA) * Math.Cos(radLatB) * Math.Pow(Math.Sin(distanceLng / 2), 2))) * EarthRadius_WGS84;

            return (int)Math.Ceiling(distance); ;
        }
    }
}
