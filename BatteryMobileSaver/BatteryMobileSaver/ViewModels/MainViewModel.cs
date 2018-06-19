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
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
        
        public int ProcessesCount { get; set; }

        public long TotalMemoryUsage { get; set; }
        public long AvailableMemoryUsage { get; set; }


        //HARDWARE
        public IHardwareInfo Hardware { get; set; }

        ChartView BatteryChartView;
        ChartView MemoryChartView;
        LineChart BatteryLineChart = new LineChart();
        DonutChart MemoryDonutChart = new DonutChart();

        public ObservableCollection<ProcessInfoModel> ProcessList { get; } = new ObservableCollection<ProcessInfoModel>();
        public ObservableCollection<AppInfoModel> AppsList { get; } = new ObservableCollection<AppInfoModel>();
        public ObservableCollection<BatteryEventViewModel> BatteryEvents { get; } = new ObservableCollection<BatteryEventViewModel>();
        List<Microcharts.Entry> BatteryChartData = new List<Microcharts.Entry>();
        List<Microcharts.Entry> MemoryChartData = new List<Microcharts.Entry>();
        MainPage MainPage;
        #endregion Fields

        #region Constructor

        public MainViewModel(IBatteryInfo battery, IHardwareInfo hardware, 
            ChartView baterryChartView, SharedViewModel sharedViewModel, ChartView memoryChartView, MainPage mainPage)
        {
            this.Battery = battery;
            this.Hardware = hardware;
            this.ProcessList = sharedViewModel.ProcessList;
            this.AppsList = sharedViewModel.AppInfoList;
            this.BatteryChartView = baterryChartView;
            this.MemoryChartView = memoryChartView;
            this.ProcessesCount = sharedViewModel.ProcessesCount;
            this.MainPage = mainPage;
            if (sharedViewModel.TotalMemoryUsage != 0)
            {
                this.TotalMemoryUsage = sharedViewModel.TotalMemoryUsage;
                this.AvailableMemoryUsage = sharedViewModel.AvailableMemoryUsage;
            }
            //Clears
            this.ClearBattery = new Command(this.BatteryEvents.Clear);
            this.ClearChart = new Command(this.BatteryChartData.Clear);
            this.KillProcesses = new Command((param) => RefreshProcesses());
        }
        #endregion Constructor
        
        public void InitMemoryChart(int isRefresh = 0)
        {
            if (isRefresh == 1)
            {
                var appsInfo = DependencyService.Get<IBackgroundAppsInfo>().GetBeackgroundProcesses();
                MemoryChartData.Clear();
                MemoryChartView.Chart = null;

                this.MemoryChartData.Add(new Microcharts.Entry(appsInfo.TotalMemoryUsage)
                {
                    Color = SKColor.Parse("#266489"),
                    ValueLabel = appsInfo.TotalMemoryUsage.ToString(),
                    Label = "Total memory usage: " + appsInfo.TotalMemoryUsage.ToString() + " MiB",
                });
                this.MemoryChartData.Add(new Microcharts.Entry(appsInfo.AvailableMemoryUsage)
                {
                    Color = SKColor.Parse("#68B9C0"),
                    ValueLabel = appsInfo.AvailableMemoryUsage.ToString(),
                    Label = "Available memory: " + appsInfo.AvailableMemoryUsage.ToString() + " MiB",
                });

                MemoryDonutChart.Entries = MemoryChartData;

                this.MemoryChartView.Chart = MemoryDonutChart;
            }
            else
            {
                this.MemoryChartData.Add(new Microcharts.Entry(TotalMemoryUsage)
                {
                    Color = SKColor.Parse("#266489"),
                    ValueLabel = TotalMemoryUsage.ToString(),
                    Label = "Total memory usage: " + TotalMemoryUsage.ToString() + " MiB",
                });
                this.MemoryChartData.Add(new Microcharts.Entry(AvailableMemoryUsage)
                {
                    Color = SKColor.Parse("#68B9C0"),
                    ValueLabel = AvailableMemoryUsage.ToString(),
                    Label = "Available memory: " + AvailableMemoryUsage.ToString() + " MiB",
                });

                MemoryDonutChart.Entries = MemoryChartData;

                this.MemoryChartView.Chart = MemoryDonutChart;
            }
        }

        public void RefreshProcesses()
        {
            var proccesses = DependencyService.Get<IBackgroundAppsInfo>().KillAvailableProcesses();
            this.ProcessList.Clear();
            foreach(var item in proccesses.ProcessList)
            {
                this.ProcessList.Add(new ProcessInfoModel
                {
                    ExeName = item.ExeName,
                    MemoryUsage = item.MemoryUsage,
                });
            }

            MainPage.DisplayMethod(this.ProcessesCount - this.ProcessList.Count);

            InitMemoryChart(1);
        }

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
                        this.BatteryChartData.Add(new Microcharts.Entry(x) { Color = SKColor.Parse("#00BFFF"), ValueLabel = DateTime.Now.ToString("HH:mm:ss") + " - " + x.ToString() + "%"});
                        BatteryLineChart.Entries = BatteryChartData;
                        this.BatteryChartView.Chart = BatteryLineChart;
                        this.TmpChartBatteryPercent = this.BatteryPercent;
                    }
                }));
        }

        void Raise(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
