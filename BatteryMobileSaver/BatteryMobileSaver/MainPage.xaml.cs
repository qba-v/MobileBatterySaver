using BatteryMobileSaver.Abstract;
using BatteryMobileSaver.ViewModels;
using Microcharts;
using Plugin.DeviceInfo;
using SkiaSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace BatteryMobileSaver
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            try
            {
                var processes = DependencyService.Get<IBackgroundAppsInfo>().GetBeackgroundProcesses();
                

                InitializeComponent();
                

                this.BindingContext = new MainViewModel(
                    CrossDevice.Battery,
                    CrossDevice.Hardware,
                    batteryChart,
                    processes
                    );
                
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((MainViewModel)this.BindingContext).Start();
        }
        
    }
}
