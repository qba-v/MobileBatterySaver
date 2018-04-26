using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using System.Collections.Generic;

namespace BatteryMobileSaver.Droid
{

    [Activity(Label = "BatteryMobileSaver", 
        Icon = "@drawable/icon", Theme = "@style/MainTheme", 
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            List<ActivityManager.RunningAppProcessInfo> processes;
            ActivityManager mgr;

            mgr = (ActivityManager)GetSystemService("ACTIVITY_SERVICE");

            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            //Xamarin.Forms.DependencyService.Register<IBackgroundApplications>();
            LoadApplication(new App());
        }
    }
}

