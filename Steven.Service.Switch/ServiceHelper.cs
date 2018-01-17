using System;
using System.Collections;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using Microsoft.Win32;

namespace Steven.Service.Switch
{
    public class ServiceHelper
    {
        // 安装服务
        public static void InstallService(string serviceName, string strServiceInstallPath)
        {
            IDictionary mySavedState = new Hashtable();
            try
            {
                if (!ServiceIsExisted(serviceName))
                {
                    // 安装服务    
                    AssemblyInstaller assemblyInstaller = new AssemblyInstaller();

                    mySavedState.Clear();
                    assemblyInstaller.Path = strServiceInstallPath;
                    assemblyInstaller.UseNewContext = true;
                    assemblyInstaller.Install(mySavedState);
                    assemblyInstaller.Commit(mySavedState);
                    assemblyInstaller.Dispose();

                    // 将服务设置为自动启动
                    ChangeServiceStartType(2, serviceName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("安装服务出错：" + ex.Message);
            }
        }

        // 卸载服务  
        public static void UnInstallService(string serviceName, string strServiceInstallPath)
        {
            try
            {
                if (ServiceIsExisted(serviceName))
                {
                    using (AssemblyInstaller assemblyInstaller = new AssemblyInstaller())
                    {
                        assemblyInstaller.UseNewContext = true;
                        assemblyInstaller.Path = strServiceInstallPath;
                        assemblyInstaller.Uninstall(null);
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("卸载服务时出错：" + ex.Message);
            }
        }

        // 卸载服务时结束主程序进程  
        public static void KillProcess(string exeName)
        {
            //后缀起始位置  
            int startpos = -1;

            try
            {
                if (exeName != "")
                {
                    if (exeName.ToLower().EndsWith(".exe"))
                    {
                        startpos = exeName.ToLower().IndexOf(".exe");
                    }

                    if (startpos < 0) return;

                    Process[] processes = Process.GetProcessesByName(exeName.Remove(startpos));

                    if (processes == null) return;

                    foreach (Process p in processes)
                    {
                        p.Kill();
                        Thread.Sleep(1000);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("强制关闭进程 {0} 出错：{1}", exeName, ex.Message));
            }
        }

        // 判断服务是否存在
        public static bool ServiceIsExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName == serviceName)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsServiceRunning(string serviceName)
        {
            if (!ServiceIsExisted(serviceName))
            {
                return false;
            }
            ServiceController service = new ServiceController(serviceName);
            if (service == null)
            {
                return false;
            }
            return service.Status == ServiceControllerStatus.Running || service.Status == ServiceControllerStatus.StartPending;
        }

        // 启动服务
        public static void StartService(string serviceName)
        {
            if (ServiceIsExisted(serviceName))
            {
                System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(serviceName);
                if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                {
                    service.Start();
                    service.Refresh();
                }
            }
        }

        // 停止服务
        public static void StopService(string serviceName)
        {
            if (ServiceIsExisted(serviceName))
            {
                System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(serviceName);
                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                    service.Refresh();
                }
            }
        }

        /// <summary>
        /// 修改服务的启动项 
        /// </summary>
        /// <param name="startType">1：自动（延迟启动，）2：自动，3：手动，4：禁用 </param>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public static bool ChangeServiceStartType(int startType, string serviceName)
        {
            try
            {
                RegistryKey regist = Registry.LocalMachine;
                RegistryKey sysReg = regist.OpenSubKey("SYSTEM");
                RegistryKey currentControlSet = sysReg.OpenSubKey("CurrentControlSet");
                RegistryKey services = currentControlSet.OpenSubKey("Services");
                RegistryKey servicesName = services.OpenSubKey(serviceName, true);
                servicesName.SetValue("Start", startType);
                servicesName.SetValue("Type", 0x00000110);
            }
            catch (Exception ex)
            {
                throw new Exception("设置启动项失败：" + ex.Message);
            }
            return true;
        } 
    }
}