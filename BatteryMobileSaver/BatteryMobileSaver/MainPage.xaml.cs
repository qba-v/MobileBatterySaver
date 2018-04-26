using BatteryMobileSaver.ViewModels;
using Microcharts;
using Plugin.DeviceInfo;
using SkiaSharp;
using System.Collections.Generic;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace BatteryMobileSaver
{
    public partial class MainPage : TabbedPage
    {

        List<Entry> entries = new List<Microcharts.Entry>
        {
            new Entry(200)
            {
                Color = SKColor.Parse("#FF1493"),
                Label = "January",
                ValueLabel = "200"
            },
            new Entry(400)
            {
                Color = SKColor.Parse("#00BFFF"),
                Label = "Febraur",
                ValueLabel = "400"
            },
            new Entry(100)
            {
                Color = SKColor.Parse("#00CED1"),
                Label = "March",
                ValueLabel = "100"
            }
        };

        public MainPage()
        {
            try
            {

                InitializeComponent();

                this.BindingContext = new MainViewModel(
                    CrossDevice.Battery,
                    CrossDevice.Hardware,
                    batteryChart
                    );

                //batteryChart.Chart = new LineChart { Entries = entries };
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
