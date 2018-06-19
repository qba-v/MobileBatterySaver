using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BatteryMobileSaver.Abstract;
using BatteryMobileSaver.Droid.AppServices;
using BatteryMobileSaver.ViewModels;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using static Android.App.ActivityManager;

[assembly: Dependency(typeof(BackgroundAppsInfoAndroid))]
namespace BatteryMobileSaver.Droid.AppServices
{
    public class BackgroundAppsInfoAndroid : IBackgroundAppsInfo
    {
        Activity currActivity = CrossCurrentActivity.Current.Activity;
        Context currContext = CrossCurrentActivity.Current.AppContext;

        int load = 0;
        public SharedViewModel GetBeackgroundProcesses()
        {
            SharedViewModel sharedViewModel = new SharedViewModel();
            ActivityManager Mgr = (ActivityManager)currContext.GetSystemService(Context.ActivityService);
            
            List<RunningAppProcessInfo> list = Mgr.RunningAppProcesses.ToList();
            foreach (var item in list)
            {
                int[] pId = { item.Pid };
                sharedViewModel.ProcessList.Add(new Models.ProcessInfoModel
                {
                    ExeName = item.ProcessName,
                    MemoryUsage = "Memory usage: " + Mgr.GetProcessMemoryInfo(pId).FirstOrDefault().TotalPss.ToString() + " KB",
                });
            }

            MemoryInfo info = new MemoryInfo();
            Mgr.GetMemoryInfo(info);
            sharedViewModel.AvailableMemoryUsage = info.AvailMem / 0x100000L;
            sharedViewModel.TotalMemoryUsage = (info.TotalMem / 0x100000L) - (info.AvailMem / 0x100000L);
            sharedViewModel.ProcessesCount = sharedViewModel.ProcessList.Count;
            return sharedViewModel;
        }

        public SharedViewModel KillAvailableProcesses()
        {
            SharedViewModel sharedViewModel = new SharedViewModel();

            ActivityManager Mgr = (ActivityManager)currContext.GetSystemService(Context.ActivityService);

            List<RunningAppProcessInfo> list = Mgr.RunningAppProcesses.ToList();
            foreach (var process in list)
            {
                if (process.PkgList.Count == 0) continue;
                try
                {
                    
                    PackageInfo packageInfo = currContext.PackageManager.GetPackageInfo(process.PkgList[0], PackageInfoFlags.Activities);

                    // Try to kill other background processes
                    // System processes are ignored
                    Mgr.KillBackgroundProcesses(packageInfo.PackageName);
                }
                catch (System.Exception ex)
                {

                    throw ex;
                }
            }
            List<RunningAppProcessInfo> listTOReturn = Mgr.RunningAppProcesses.ToList();

            foreach (var item in listTOReturn)
            {
                int[] pId = { item.Pid };
                sharedViewModel.ProcessList.Add(new Models.ProcessInfoModel
                {
                    ExeName = item.ProcessName,
                    MemoryUsage = "Memory usage: " + Mgr.GetProcessMemoryInfo(pId).FirstOrDefault().TotalPss.ToString() + " KB",
                });
            }
            sharedViewModel.ProcessesCount = sharedViewModel.ProcessList.Count;
            return sharedViewModel;
        }
    }
}