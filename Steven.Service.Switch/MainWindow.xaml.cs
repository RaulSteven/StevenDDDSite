using System.Windows;
using log4net;
using Steven.Service.WinServer.Common;

namespace Steven.Service.Switch
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ILog Log;
        public MainWindow()
        {
            InitializeComponent();
            Log = LogManager.GetLogger(this.GetType().FullName);

            this.CheckStatus();
        }

        private void CheckStatus()
        {
            Log.Info("开始检查状态");
            this.btnRemove.IsEnabled = false;
            this.btnSetup.IsEnabled = false;
            this.btnStart.IsEnabled = false;
            this.btnStop.IsEnabled = false;
            if (ServiceHelper.ServiceIsExisted(AppGlobal.SvcName))
            {
                this.btnRemove.IsEnabled = true;

                if (ServiceHelper.IsServiceRunning(AppGlobal.SvcName))
                {
                    this.btnStop.IsEnabled = true;
                }
                else
                {
                    this.btnStart.IsEnabled = true;
                }
            }
            else
            {
                this.btnSetup.IsEnabled = true;
            }
            Log.Info("结束检查状态");
        }

        private void btnSetup_Click(object sender, RoutedEventArgs e)
        {
            // 卸载服务
            ServiceHelper.UnInstallService(AppGlobal.SvcName, AppGlobal.SvcInstallPath);
            ServiceHelper.KillProcess(AppGlobal.AppExeName);

            // 安装服务
            ServiceHelper.InstallService(AppGlobal.SvcName, AppGlobal.SvcInstallPath);
            this.CheckStatus();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            // 卸载服务
            ServiceHelper.UnInstallService(AppGlobal.SvcName, AppGlobal.SvcInstallPath);
            ServiceHelper.KillProcess(AppGlobal.AppExeName);
            this.CheckStatus();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            ServiceHelper.StartService(AppGlobal.SvcName);
            this.CheckStatus();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            ServiceHelper.StopService(AppGlobal.SvcName);
            this.CheckStatus();
        }
    }
}
