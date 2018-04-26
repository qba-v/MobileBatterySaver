using BatteryMobileSaver.Models;
using Microcharts;
using Microcharts.Forms;
using Plugin.DeviceInfo;
using PropertyChanged;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace BatteryMobileSaver.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields
        public event PropertyChangedEventHandler PropertyChanged;
        bool firstStart = true;

        //BATERRY
        public IBatteryInfo Battery { get; private set; }
        public PowerStatus BatteryStatus { get; set; }
        public ICommand ClearBattery { get; }
        public ICommand ClearChart { get; }
        IDisposable batteryPower;
        IDisposable batteryPercent;
        public int BatteryPercent { get; set; }

        //HARDWARE
        public IHardwareInfo Hardware { get; set; }

        ChartView ChartView;
        LineChart lineChart = new LineChart();
        #endregion Fields

        #region Constructor

        public MainViewModel(IBatteryInfo battery, IHardwareInfo hardware, ChartView chartView)
        {
            this.Battery = battery;
            this.Hardware = hardware;
            this.ChartView = chartView;
            //Clears
            this.ClearBattery = new Command(this.BatteryEvents.Clear);
            this.ClearChart = new Command(this.ChartData.Clear);
        }
        #endregion Constructor

        #region Collections
        public ObservableCollection<BatteryEventViewModel> BatteryEvents { get; } = new ObservableCollection<BatteryEventViewModel>();
        List<Microcharts.Entry> ChartData = new List<Microcharts.Entry>();
        #endregion Collections

        public void Start()
        {
            if (!this.firstStart)
                return;

            this.batteryPower = this.Battery
               .WhenPowerStatusChanged()
               .Subscribe(x => Device.BeginInvokeOnMainThread(() =>
               {
                   this.BatteryStatus = x;
                   this.BatteryEvents.Insert(0, new BatteryEventViewModel
                   {
                       Detail = $"Status Change: {x}"
                   });
               }));

            this.batteryPercent = this.Battery
                .WhenBatteryPercentageChanged()
                .Subscribe(x => Device.BeginInvokeOnMainThread(() =>
                {
                    this.BatteryPercent = x;
                    this.BatteryEvents.Insert(0, new BatteryEventViewModel
                    {
                        Detail = $"Charge Change: {x}%"
                    });

                    this.ChartData.Add(new Microcharts.Entry(x) { Color = SKColor.Parse("#00BFFF"), ValueLabel = x.ToString() });
                    lineChart.Entries = ChartData;
                    this.ChartView.Chart = lineChart;
                }));
        }

        void Raise(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
