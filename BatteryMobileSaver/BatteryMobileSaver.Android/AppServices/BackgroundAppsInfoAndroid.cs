using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        int load = 0;
        public UWPViewModel GetBeackgroundProcesses()
        {
            UWPViewModel uwpViewModel = new UWPViewModel();
            var tmp = CrossCurrentActivity.Current.Activity;
            var mgr = MainActivity.Mgr;

            List<RunningAppProcessInfo> list = mgr.RunningAppProcesses.ToList();
            foreach (var item in list)
            {
                uwpViewModel.ProcessList.Add(new Models.ProcessInfoModel
                {
                    ExeName = item.ProcessName,
                });
            }
            return uwpViewModel;
        }

        public UWPViewModel KillAvailableProcesses()
        {
            UWPViewModel uwpViewModel = new UWPViewModel();
            List<RunningAppProcessInfo> list = MainActivity.Mgr.RunningAppProcesses.ToList();
            foreach (var process in list)
            {
                if (process.PkgList.Count == 0) continue;
                try
                {
                    PackageInfo packageInfo = MainActivity.packageManager.GetPackageInfo(process.PkgList[0], PackageInfoFlags.Activities);

                    // Try to kill other background processes
                    // System processes are ignored
                    MainActivity.Mgr.KillBackgroundProcesses(packageInfo.PackageName);
                }
                catch (System.Exception ex)
                {

                    throw ex;
                }
            }
            List<RunningAppProcessInfo> listTOReturn = MainActivity.Mgr.RunningAppProcesses.ToList();

            foreach (var item in list)
            {
                uwpViewModel.ProcessList.Add(new Models.ProcessInfoModel
                {
                    ExeName = item.ProcessName,
                });
            }

            return uwpViewModel;
        }
    }
}