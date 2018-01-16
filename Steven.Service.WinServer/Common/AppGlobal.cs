using System;
using System.Configuration;
using System.IO;

namespace Steven.Service.WinServer.Common
{
    public class AppGlobal
    {
        private static string _SvcName = null;
        public static string SvcName
        {
            get
            {
                if (_SvcName == null)
                {
                    _SvcName = ConfigurationManager.AppSettings["SvcName"];
                }
                return _SvcName;
            }
        }

        private static string _SvcInstallPath = null;
        public static string SvcInstallPath
        {
            get
            {
                if (_SvcInstallPath == null)
                {
                    _SvcInstallPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppExeName);
                }

                return _SvcInstallPath;
            }
        }

        private static string _AppExeName = null;
        public static string AppExeName
        {
            get
            {
                if (_AppExeName == null)
                {
                    _AppExeName = ConfigurationManager.AppSettings["AppExeName"];
                }

                return _AppExeName;
            }
        }
    }
}