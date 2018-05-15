using BatteryMobileSaver.Abstract;
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
        public ICommand KillProcesses { get; }
        IDisposable batteryPower;
        IDisposable batteryPercent;
        public int BatteryPercent { get; set; }
        public int TmpChartBatteryPercent { get; set; }

        //HARDWARE
        public IHardwareInfo Hardware { get; set; }

        ChartView ChartView;
        LineChart lineChart = new LineChart();
        public ObservableCollection<ProcessInfoModel> ProcessList { get; } = new ObservableCollection<ProcessInfoModel>();
        public ObservableCollection<AppInfoModel> AppsList { get; } = new ObservableCollection<AppInfoModel>();
        public ObservableCollection<BatteryEventViewModel> BatteryEvents { get; } = new ObservableCollection<BatteryEventViewModel>();
        List<Microcharts.Entry> ChartData = new List<Microcharts.Entry>();
        #endregion Fields

        #region Constructor

        public MainViewModel(IBatteryInfo battery, IHardwareInfo hardware, ChartView chartView, UWPViewModel uwpViewModel)
        {
            this.Battery = battery;
            this.Hardware = hardware;
            this.ChartView = chartView;
            this.ProcessList = uwpViewModel.ProcessList;
            this.AppsList = uwpViewModel.AppInfoList;
            //Clears
            this.ClearBattery = new Command(this.BatteryEvents.Clear);
            this.ClearChart = new Command(this.ChartData.Clear);
            this.KillProcesses = new Command((param) => DependencyService.Get<IBackgroundAppsInfo>().KillAvailableProcesses());
        }
        #endregion Constructor
        

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
                    if (this.TmpChartBatteryPercent != this.BatteryPercent)
                    {
                        this.ChartData.Add(new Microcharts.Entry(x) { Color = SKColor.Parse("#00BFFF"), ValueLabel = x.ToString() });
                        lineChart.Entries = ChartData;
                        this.ChartView.Chart = lineChart;
                        this.TmpChartBatteryPercent = this.BatteryPercent;
                    }
                }));
        }

        void Raise(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
