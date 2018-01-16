using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Admin.Models
{
    public class HomeDataModel
    {
        public int TotalOrderCount { get; set; }

        public int TotalNewOrderCount { get; set; }

        public string TotalNewOrderPercent { get; set; }

        public int TotalUserCount { get; set; }

        public int TotalMonthUserCount { get; set; }

        public string TotalMonthUserPercent { get; set; }
    }

    public class HomeStatisticsDataModel
    {
        public int TotalOrderCount { get; set; }
        public float TotalOrderPercent { get; set; }

        public int TotalLastMonthOrderCount { get; set; }
        public float TotalLastMonthOrderPercent { get; set; }

        public int TotalOrderIncome { get; set; }
        public float TotalOrderIncomePercent { get; set; }

        public string StatisticsData { get; set; }
    }
}