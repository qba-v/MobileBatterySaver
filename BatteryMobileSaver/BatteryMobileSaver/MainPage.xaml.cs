using Android.Content.PM;
using BatteryMobileSaver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace BatteryMobileSaver
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btn_Clicked(object sender, EventArgs e)
        {
            List<string> list = new List<string>();

            var apps = Android.App.Application.Context.PackageManager.GetInstalledApplications(PackageInfoFlags.MatchAll);
            
            for(int i = 0; i<apps.Count; i++)
            {
                var app = apps[i].LoadLabel(Android.App.Application.Context.PackageManager);

                list.Add(app);
            }

            DependencyService.Get<IBackgroundApplications>().GetBackgroundApplications();

        }
    }
}
