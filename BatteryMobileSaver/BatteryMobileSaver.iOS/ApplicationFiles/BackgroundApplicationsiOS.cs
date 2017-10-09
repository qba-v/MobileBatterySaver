using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using BatteryMobileSaver.Interfaces;
using BatteryMobileSaver.iOS.ApplicationFiles;

[assembly: Xamarin.Forms.Dependency(typeof(BackgroundApplicationsiOS))]
namespace BatteryMobileSaver.iOS.ApplicationFiles
{
    public class BackgroundApplicationsiOS : IBackgroundApplications
    {
        public BackgroundApplicationsiOS() {}

        public void GetBackgroundApplications()
        {
            throw new NotImplementedException();
        }
    }
}