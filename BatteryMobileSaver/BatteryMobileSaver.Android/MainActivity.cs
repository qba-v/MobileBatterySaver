﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using System.Collections.Generic;
using Android.Content;
using static Android.App.ActivityManager;
using System.Linq;
using Java.Lang;

namespace BatteryMobileSaver.Droid
{

    [Activity(Label = "BatteryMobileSaver",
        Icon = "@drawable/icon", Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        List<ActivityManager.RunningAppProcessInfo> processes;
        ActivityManager mgr;
        int load = 0;
        Android.Widget.ListView lst;
        MyAdapter adp;

        private Context mContext;

        protected override void OnCreate(Bundle bundle)
        {
            lst = new Android.Widget.ListView(this);
            mgr = (ActivityManager)GetSystemService(Context.ActivityService);

            mContext = this;

            string s = string.Empty;

            List<RunningAppProcessInfo> list = mgr.RunningAppProcesses.ToList();

            // Count the number of running processes
            int initialRunningProcessesSize = list.Count;

            // Iterate over the RunningAppProcess list
            foreach (var process in list)
            {
                if (process.PkgList.Count == 0) continue;
                try
                {
                    PackageInfo packageInfo = PackageManager.GetPackageInfo(process.PkgList[0], PackageInfoFlags.Activities);

                    if (!(packageInfo.PackageName == this.PackageName))
                    {
                        // Try to kill other background processes
                        // System processes are ignored
                        mgr.KillBackgroundProcesses(packageInfo.PackageName);
                    }

                }
                catch (System.Exception ex)
                {

                    throw ex;
                }
            }

            // Get the running processes after killing some
            int currentRunningProcessesSize = mgr.RunningAppProcesses.Count;

            // Return the number of killed processes
            var count = initialRunningProcessesSize - currentRunningProcessesSize;

            //KillBackgroundProcessesTask km = new KillBackgroundProcessesTask(mContext, PackageManager).Execute(Java.Lang.Object[]);

            foreach (var item in mgr.RunningAppProcesses)
            {
                if (item.ProcessName != "com.acrapps.deviceinfo")
                {
                    Android.OS.Process.KillProcess(item.Pid);
                    Android.OS.Process.SendSignal(item.Pid, Signal.Kill);
                    mgr.RestartPackage(item.ProcessName);
                    mgr.KillBackgroundProcesses(item.ProcessName);
                }
                list.Add(item);
            }

            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            //Xamarin.Forms.DependencyService.Register<IBackgroundApplications>();
            LoadApplication(new App());
        }



        //private class KillBackgroundProcessesTask : AsyncTask //<Void, Void, Void>
        //{
        //    Context mmContext;
        //    PackageManager Manager;

        //    public KillBackgroundProcessesTask()
        //    {

        //    }
            

        //    public KillBackgroundProcessesTask(Context context, PackageManager manager)
        //    {
        //        mmContext = context;
        //        Manager = manager;
        //    }

        //    protected void onPostExecute(int result)
        //    {

        //        // Show the number of killed processes
        //        //Toast.MakeText(mContext, "Killed : " + result + " processes", Toast.LENGTH_SHORT).show();

        //        // Refresh the TextView with running processes
        //        //populateTextViewWithRunningProcesses();
        //    }

        //    protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
        //    {
        //        // Get an instance of ActivityManager
        //        ActivityManager am = (ActivityManager)mmContext.GetSystemService(Context.ActivityService);


        //        // Get a list of RunningAppProcessInfo
        //        List<ActivityManager.RunningAppProcessInfo> list = am.RunningAppProcesses.ToList();

                
        //    }

        //    //protected override int RunInBackground(params void[] @params)
        //    //{
        //    //    throw new NotImplementedException();
        //    //}
        //}

    }
    
}

