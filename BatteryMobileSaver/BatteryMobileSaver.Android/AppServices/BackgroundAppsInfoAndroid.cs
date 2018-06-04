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
                sharedViewModel.ProcessList.Add(new Models.ProcessInfoModel
                {
                    ExeName = item.ProcessName,
                });
            }
            sharedViewModel.ProcessesCount = sharedViewModel.ProcessList.Count;
            return sharedViewModel;
        }

        public Task<SharedViewModel> GetBeackgroundProcesses1()
        {
            throw new NotImplementedException();
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
                sharedViewModel.ProcessList.Add(new Models.ProcessInfoModel
                {
                    ExeName = item.ProcessName,
                });
            }
            sharedViewModel.ProcessesCount = sharedViewModel.ProcessList.Count;
            return sharedViewModel;
        }
    }
}