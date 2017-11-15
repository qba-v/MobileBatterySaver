using BatteryMobileSaver.Models;
using LibDeviceInfo;
using PropertyChanged;
using System;
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
        public IBatteryInfo Battery { get; private set; }
        public PowerStatus BatteryStatus { get; set; }
        public int BatteryPercent { get; set; }
        public ICommand ClearBattery { get; }

        public MainViewModel(IBatteryInfo battery)
        {
            this.Battery = battery;

            this.ClearBattery = new Command(this.BatteryEvents.Clear);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<BatteryEventViewModel> BatteryEvents { get; } = new ObservableCollection<BatteryEventViewModel>();
        

        bool firstStart = true;
        IDisposable batteryPower;
        IDisposable batteryPercent;

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
                }));
        }

        void Raise(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
