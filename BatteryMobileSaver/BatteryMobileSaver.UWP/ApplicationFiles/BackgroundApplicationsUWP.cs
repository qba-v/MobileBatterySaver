using BatteryMobileSaver.UWP.ApplicationFiles;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(BackgroundApplicationsUWP))]
namespace BatteryMobileSaver.UWP.ApplicationFiles
{
    public class BackgroundApplicationsUWP : IBackgroundApplications
    {
        public BackgroundApplicationsUWP() {}

        
        public void GetBackgroundApplications()
        {
            throw new NotImplementedException();
        }
    }
}
