using Android.Content.PM;
using BatteryMobileSaver.Interfaces;
using BatteryMobileSaver.Models;
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

            var button = new Button
            {
                Text = "Click for battery info",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            
        }

        private void btn_Clicked(object sender, EventArgs e)
        {
            //List<string> list = new List<string>();

            //var apps = Android.App.Application.Context.PackageManager.GetInstalledApplications(PackageInfoFlags.MatchAll);

            //for(int i = 0; i<apps.Count; i++)
            //{
            //    var app = apps[i].LoadLabel(Android.App.Application.Context.PackageManager);

            //    list.Add(app);
            //}

            //DependencyService.Get<IBackgroundApplications>().GetBackgroundApplications();

            var bat = DependencyService.Get<IBattery>();

            switch (bat.PowerSource)
            {
                case PowerSource.Battery:
                    btn.Text = "Battery - ";
                    break;
                case PowerSource.Ac:
                    btn.Text = "AC - ";
                    break;
                case PowerSource.Usb:
                    btn.Text = "USB - ";
                    break;
                case PowerSource.Wireless:
                    btn.Text = "Wireless - ";
                    break;
                case PowerSource.Other:
                default:
                    btn.Text = "Other - ";
                    break;
            }
            switch (bat.Status)
            {
                case BatteryStatus.Charging:
                    btn.Text += $"Charging. Now your battery is : {bat.RemainingChargePercent} percent";
                    break;
                case BatteryStatus.Discharging:
                    btn.Text += "Discharging";
                    break;
                case BatteryStatus.NotCharging:
                    btn.Text += "Not Charging";
                    break;
                case BatteryStatus.Full:
                    btn.Text += "Full";
                    break;
                case BatteryStatus.Unknown:
                default:
                    btn.Text += "Unknown";
                    break;
            }
            Content = btn;
        }
    }
}
