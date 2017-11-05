
using BatteryMobileSaver.Models;
using LibDeviceInfo.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace BatteryMobileSaver.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public IBatteryInfo Battery { get; private set; }
        public ICommand ClearBattery { get; }

        public MainViewModel(IBatteryInfo battery)
        {
            this.Battery = battery;

            this.ClearBattery = new Command(this.BatteryEvents.Clear);
        }

        public ObservableCollection<BatteryEventViewModel> BatteryEvents { get; } = new ObservableCollection<BatteryEventViewModel>();

        public event PropertyChangedEventHandler PropertyChanged;
        void Raise(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
