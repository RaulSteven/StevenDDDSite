using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Steven.Service.Tasks;
using Steven.Service.Tasks.Infrastructure;
using Steven.Service.Tasks.Jobs;

namespace Steven.WinTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void BtnStartTask_Click(object sender, RoutedEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            TaskManager.Start();
        }
        private void BtnWeixinNotify_Click(object sender, RoutedEventArgs e)
        {
            DependencyConfig.Register();
            var job = new WeixinNotifyJob();
            job.ExcuteTask();
        }
    }
}
