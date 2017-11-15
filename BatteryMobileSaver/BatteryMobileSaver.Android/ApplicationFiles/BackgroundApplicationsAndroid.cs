
using BatteryMobileSaver.Droid.ApplicationFiles;
using BatteryMobileSaver.Interfaces;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(BackgroundApplicationsAndroid))]
namespace BatteryMobileSaver.Droid.ApplicationFiles
{
    public class BackgroundApplicationsAndroid : IBackgroundApplications
    {
        public BackgroundApplicationsAndroid() {}

        public void GetBackgroundApplications()
        {
            
        }
    }
}